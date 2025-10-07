using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    private int score = 0;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        ResetAllBrickAnimators();
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
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