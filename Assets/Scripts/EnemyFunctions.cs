using UnityEngine;

public class EnemyFunctions : MonoBehaviour
{

    public AudioSource stompedAudio;
    void HideEnemy()
    {
        gameObject.SetActive(false);
    }

    void ShowEnemy()
    {
        gameObject.SetActive(true);
    }

    void GoombaStomped()
    {
        stompedAudio.time = 0.5f;
        stompedAudio.PlayOneShot(stompedAudio.clip);
    }
}
