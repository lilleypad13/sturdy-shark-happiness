using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static bool gameHasEnded;

    private int level;

    public int totalLevels; //Only necessary for CompleteLevel Script to determine if there are no more leves, and it should just restart the game
    public Text levelText;
    public GameObject loseScreen;

    public GameObject player;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                new GameManager();
            return instance;
        }
    }
    private static GameManager instance;
    private GameManager()
    {
        if (instance != null)
            return;
        instance = this;
    }


    private void Start()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        totalLevels -= 1; //Used to get the final build index number
        //levelText.text = "Level " + (level + 1);
        gameHasEnded = false;
    }

    public void WinGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("You have won!");
            SceneManager.LoadScene(3);
        }
    }


    //public void CompleteLevel()
    //{
    //    if (gameHasEnded == false)
    //    {
    //        if (level >= totalLevels)     // Checks if the upcoming level would be greater than the total number of levels, so it can return the player to the beginning of the game.
    //        {
    //            gameHasEnded = true;
    //            level = 0;
    //            completeLevelUI.SetActive(true);
    //        }
    //        else // This just advances the player to the next level
    //        {
    //            level += 1;
    //            gameHasEnded = true;
    //            nextLevelUI.SetActive(true);
    //        }

    //    }
    //}

    public void LoseGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            loseScreen.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(1);
        // Will want to replace this with the build index of our "Game Over" screen
    }
}
