using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;

public class LoadingSceneController : MonoBehaviour
{
    public Transform mario;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public float walkSpeed = 3f;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.6f;
    public TMP_Text worldText;
    public string nextSceneName = "World 1-1";
    public string prevSceneName = "MainMenu";
    public CanvasGroup canvas;

    void Start()
    {
        worldText.alpha = 0;
        marioAnimator.SetBool("onGround", true);
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        Time.timeScale = 1f;

        marioAnimator.SetFloat("xSpeed", 0.06f);

        while (mario.position.x < 0f)
        {
            mario.position += Vector3.right * walkSpeed * Time.deltaTime;
            yield return null;
        }

        marioAnimator.SetFloat("xSpeed", 0f);

        yield return new WaitForSeconds(0.3f); // tiny pause before jump

        marioAnimator.SetBool("onGround", false);
        yield return StartCoroutine(JumpMario());
        marioAnimator.SetBool("onGround", true);

        yield return StartCoroutine(FadeInText());

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        SpriteRenderer marioSprite = mario.GetComponent<SpriteRenderer>();
        Color marioColor = marioSprite.color;

        for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
        {
            canvas.alpha = alpha;
            marioSprite.color = new Color(marioColor.r, marioColor.g, marioColor.b, alpha);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // once done, go to next scene
        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }

    IEnumerator JumpMario()
    {
        float elapsed = 0f;
        Vector3 start = mario.position;

        while (elapsed < jumpDuration)
        {
            float progress = elapsed / jumpDuration;
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            mario.position = start + Vector3.up * height;
            elapsed += Time.deltaTime;
            yield return null;
        }

        mario.position = start; // reset to ground
    }

    IEnumerator FadeInText()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            worldText.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        worldText.alpha = 1;
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
        // if (marioAudio.isPlaying) Debug.Log("currently playing: " + marioAudio.clip.name);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadSceneAsync(prevSceneName, LoadSceneMode.Single);
    }
}
