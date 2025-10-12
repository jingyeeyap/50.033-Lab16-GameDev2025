using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public IntVariable gameScore;
    public GameObject highscoreText;
    public string nextSceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetHighscore();
    }

    // Update is called once per frame
    public void GoToLoadScreen()
    {
        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }

    public void ResetHighscore()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        gameScore.previousHighestValue = 0;
        SetHighscore();
    }

    void SetHighscore()
    {
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
    }
}
