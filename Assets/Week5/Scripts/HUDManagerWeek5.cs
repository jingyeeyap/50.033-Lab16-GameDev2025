using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManagerWeek5 : MonoBehaviour
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
    public GameObject highscoreText;
    public IntVariable gameScore;
    public Image pauseButtonImage;
    public string MainMenuName = "MainMenuLab5";

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        pauseButtonImage.transform.parent.gameObject.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value.ToString();
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
        SceneManager.LoadSceneAsync(MainMenuName, LoadSceneMode.Single);
    }
}
