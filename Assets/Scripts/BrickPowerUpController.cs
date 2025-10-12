using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    // public PowerupContainerType containerType;
    float coinChance = 0.5f;
    public BasePowerup powerup; // reference to this brick's powerup

    void Start()
    {
        // this.containerType = PowerupContainerType.UnbreakableBrick;
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // get first contact point
            ContactPoint2D contact = other.contacts[0];

            // Vector from player to block
            Vector2 contactNormal = contact.normal;

            // If the collision normal points downward, the player hit from below
            if (contactNormal.y > 0.5f)
            {
                // Debug.Log("Block hit from below!");

                this.GetComponent<Animator>().SetTrigger("marioHit");

                if (Random.value < coinChance)
                {
                    powerup.GetComponent<CoinPowerup>().playSound = true;
                    powerup.GetComponent<Animator>().SetTrigger("startAnim");
                }
                else
                {
                    powerup.GetComponent<CoinPowerup>().playSound = false;
                }
            }
            // spawn the powerup
            // powerupAnimator.SetTrigger("spawned");
        }
    }

    // used by animator
    public void Disable()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void GameRestart()
    {
        GetComponent<Animator>().SetTrigger("gameRestart");
    }

}