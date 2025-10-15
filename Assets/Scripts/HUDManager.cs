using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-700, 473, 0),
        new Vector3(0, -50, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(844, 455, 0),
        new Vector3(0, -200, 0)
    };
    public GameObject scoreText;
    public Transform restartButton;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject highscoreText;
    public IntVariable gameScore;
    private bool isPaused = false;
    public Sprite[] pauseSprites;
    public Image pauseButtonImage;
    public AudioSource bgmAudio;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        pauseButtonImage.transform.parent.gameObject.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];

        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);

        // set the parent of the button image to disabled
        pauseButtonImage.transform.parent.gameObject.SetActive(false);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        // freeze game time
        Time.timeScale = 0f;

        if (bgmAudio != null && bgmAudio.isPlaying)
            bgmAudio.Pause();

        // show pause menu
        pausePanel.SetActive(true);
        pauseButtonImage.sprite = pauseSprites[1];

        isPaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (bgmAudio != null)
            bgmAudio.UnPause();

        pausePanel.SetActive(false);
        pauseButtonImage.sprite = pauseSprites[0];

        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
}
