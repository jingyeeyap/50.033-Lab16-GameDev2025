using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerWeek5 : MonoBehaviour
{
    public GameConstants gameConstants;
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent updateScore;
    // public UnityEvent gameOver;
    Transform player; // Mario's Transform
    public IntVariable gameScore;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameStart.Invoke();
        // gameScore.Value = 0;
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
        gameConstants.marioAlive = true;
    }

    public void GameStart()
    {
        // gameConstants.marioAlive = true;
        gameScore.Value = 0;
    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();

        if (next.name == "MainMenu" || next.name == "LoadingScreen")
        {
            gameScore.Value = 0;
        }

        SetScore();
        Time.timeScale = 1.0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void GameRestart()
    {
        // reset score
        gameScore.Value = 0;
        SetScore();
        // gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        // score += increment;
        // increase score by 1
        gameScore.ApplyChange(increment);
        SetScore();
    }

    public void SetScore()
    {
        // invoke score change event with current score to update HUD
        // UpdateScore.Invoke(gameScore.Value);
        updateScore.Invoke();
    }

    public void RequestPowerup(IPowerup powerup)
    {
        if (powerup.powerupType == PowerupType.Coin)
            IncreaseScore(1);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
    }
}