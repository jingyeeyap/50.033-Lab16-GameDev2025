using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public Transform player; // Mario's Transform
    public IntVariable gameScore;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameStart.Invoke();
        gameScore.Value = 0;
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
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
        gameRestart.Invoke();
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
        scoreChange.Invoke(gameScore.Value);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}