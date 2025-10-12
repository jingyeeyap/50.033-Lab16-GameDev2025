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
        SetScore();
    }

    public void GameRestart()
    {
        // reset score
        gameScore.Value = 0;
        SetScore();
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        // ResetAllBrickAnimators();
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

    public Transform obstacles; // assign in Inspector (e.g. root object)
    void ResetAllBrickAnimators()
    {
        if (obstacles == null)
        {
            Debug.LogWarning("No parent assigned!");
            return;
        }

        Animator[] animators = obstacles.GetComponentsInChildren<Animator>(true);
        // (true) includes disabled objects too

        foreach (Animator anim in animators)
        {
            if (anim.gameObject.CompareTag("QuestionBox"))
            {
                anim.SetTrigger("gameRestart");
                // clear it immediately so it won’t interfere with other transitions
                anim.ResetTrigger("marioHit");
            }
        }
    }
}