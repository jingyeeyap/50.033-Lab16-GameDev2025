using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameObject coin;
    private AudioSource coinAudio;
    float coinChance = 0.5f;
    // public Animator brickOrBoxAnimator;
    public PlayerMovement playerMovement;

    // void Update()
    // {
    //     if (playerMovement.restartQuestionBox)
    //     {
    //         this.GetComponent<Animator>().SetTrigger("restartGame");
    //         playerMovement.restartQuestionBox = false;
    //     }
    // }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // get first contact point
            ContactPoint2D contact = collision.contacts[0];

            // Vector from player to block
            Vector2 contactNormal = contact.normal;

            // If the collision normal points downward, the player hit from below
            if (contactNormal.y > 0.5f)
            {
                // Debug.Log("Block hit from below!");

                this.GetComponent<Animator>().SetTrigger("marioHit");
            }
        }
    }

    // randomize the coin spawn for bricks 
    void SpawnCoin()
    {
        if (Random.value < coinChance)
        {
            PlayCoinSound();
        }
    }

    void PlayCoinSound()
    {
        coinAudio = coin.GetComponent<AudioSource>();
        coinAudio.PlayOneShot(coinAudio.clip);
        coin.GetComponent<Animator>().SetTrigger("startAnim");
    }

}
