using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameObject interactableObject;
    private AudioSource objectAudio;
    float coinChance = 0.5f;
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerMovement.alive)
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
        objectAudio = interactableObject.GetComponent<AudioSource>();
        objectAudio.PlayOneShot(objectAudio.clip);
        interactableObject.GetComponent<Animator>().SetTrigger("startAnim");
    }

    void PlayMushroomSound()
    {
        objectAudio = interactableObject.GetComponent<AudioSource>();
        objectAudio.PlayOneShot(objectAudio.clip);
        interactableObject.GetComponent<Animator>().SetTrigger("spawn-mushroom");
    }

}
