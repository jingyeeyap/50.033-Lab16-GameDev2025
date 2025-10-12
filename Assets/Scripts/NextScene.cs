using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    public GameObject HUD;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Change scene!");
            // DontDestroyOnLoad(other.gameObject);
            StartCoroutine(LoadNextScene(other.gameObject));
        }
    }

    IEnumerator LoadNextScene(GameObject player)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
        if (nextScene.IsValid())
        {
            // DontDestroyOnLoad(HUD);
            // SceneManager.MoveGameObjectToScene(player, nextScene);
            // SceneManager.MoveGameObjectToScene(HUD, nextScene);
            Debug.Log("Player moved to new scene!");
        }
        else
        {
            Debug.LogError("Failed to find loaded scene!");
        }
    }
}
