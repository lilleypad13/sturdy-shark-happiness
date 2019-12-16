using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public bool displayGridGizmos;
    //public bool onlyDisplayPathGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize; // area in world coordinates that grid will cover
    public float nodeRadius; // how much space each individual node covers
    public TerrainType[] walkableRegions;
    public int obstacleProximityPenalty = 10;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    Node[,] grid; //2D array of nodes

    float nodeDiamater;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    // Determines how many nodes can be fit into the grid based on the overall grid size 
    // and the size of the individual nodes
    private void Awake()
    {
        nodeDiamater = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiamater);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiamater);

        foreach (TerrainType region in walkableRegions)
        {
            //walkableMask.value = walkableMask | region.terrainMask.value;
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY]; // Creates a 2D grid that gives the total number of nodes
        // This point represents the bottom left corner of the grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Determines the actual position of the node
                Vector3 worldPoint = worldBottomLeft +
                    Vector3.right * (x * nodeDiamater + nodeRadius) + 
                    Vector3.up * (y * nodeDiamater + nodeRadius);

                // Determine if node is traversable
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                Debug.Log("The unwalkable mask is: " + LayerMask.LayerToName(1));
                Debug.Log("Walkable is: " + walkable);

                int movementPenalty = 0;

                // Raycast
                Ray ray = new Ray(worldPoint + Vector3.forward * 50, Vector3.back);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                if (!walkable)
                    movementPenalty += obstacleProximityPenalty;

                // Create node and assign determined values
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(3);
    }

    private void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        //int kernelExtents = (kernelSize - 1) / 2;
        int kernelExtents = blurSize;

        // Creates two new grids of equal size to the original grid to hold the updated blurred values.
        // The horizontal pass uses values from the original grid to fill its own first, 
        // then the vertical pass uses the values from the horizontal grid to fill itself after.
        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        // All of the Mathf.Clamp methods used are to effectively reproduce values at the boundaries of the grid 
        // to keep anything from going out of the grid bounds upon calculation.
        for (int y = 0; y < gridSizeY; y++)
        {
            // Fills the first element in the row based on the kernel size
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            // Fills the rest of the elements after the first
            // As this process moves throughout the grid, it starts with the last value calculated in the grid and subtracts 
            // the leftmost elements from that value (that were dropped from the kernel) and adds the new rightmost elements 
            // (newly added to the kernel upon advancing) to obtain the value for the current grid location.
            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                // Generates the next value by simply subtracting the value from the single element 
                // removed from the kernel and adding the value from the newly added element
                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] 
                    - grid[removeIndex, y].movementPenalty
                    + grid[addIndex, y].movementPenalty;
            }
        }

        // Same process, but reverse x and y for vertical process
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1]
                    - penaltiesHorizontalPass[x, removeIndex]
                    + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                // Solely for visualization purposes
                if(blurredPenalty > penaltyMax)
                    penaltyMax = blurredPenalty;
                if(blurredPenalty < penaltyMin)
                    penaltyMin = blurredPenalty;
            }
        }
    }
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) // This is the node itself, which does not need checked
                    continue;

                // Ensures the node exists by checking if it is within the overall bounds of the grid
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && 
                    checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    /*
     * Uses a vector3 position to determine which node encompasses that position.
     * If out of bounds, will simply return the extremes of the outter edge of the grid.
     */
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        // Clamped to prevent values from going out of bounds (will never be less than 0 or greater than 1)
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Uses percent values to determine index values for node in array
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null && displayGridGizmos == true)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiamater);
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
