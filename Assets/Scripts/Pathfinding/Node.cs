using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int movementPenalty;
    public Node parent;

    // Added
    public int heapIndex;

    public int gCost;
    public int hCost;
    // fCost will never be anything else, so no reason to directly assign it
    public int fCost { get { return gCost + hCost; } } 

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    /*
     * Compares the fCost of this node and another node, and if they are equal, compares 
     * the hCost of the two nodes.
     * It returns the opposite of this compare value as a higher priority 
     * is represented by lower costs.
     */
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
