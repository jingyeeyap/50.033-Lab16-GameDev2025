
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseButtonControllerWeek5 : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;
    public Sprite pauseIcon;
    public Sprite playIcon;
    private Image image;
    public GameObject pausePanel;
    public AudioSource bgmAudio;
    public UnityEvent gamePaused;
    public UnityEvent gameResumed;
    // Start is called before the first frame update
    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }

    public void ButtonClick()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // freeze game time
        Time.timeScale = 0f;

        if (bgmAudio != null && bgmAudio.isPlaying)
            bgmAudio.Pause();

        // show pause menu
        pausePanel.SetActive(true);
        image.sprite = playIcon;

        isPaused = true;
        gamePaused.Invoke();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (bgmAudio != null)
            bgmAudio.UnPause();

        pausePanel.SetActive(false);
        image.sprite = pauseIcon;

        isPaused = false;
        gameResumed.Invoke();
    }
}
