using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupControllerWeek5 : MonoBehaviour, IPowerupController
{
    public GameConstants gameConstants;
    public Animator powerupAnimator;
    // public PowerupContainerType containerType;
    public BasePowerup powerup; // reference to this question box's powerup

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerup.hasSpawned && gameConstants.marioAlive)
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

                if (powerup.type == PowerupType.MagicMushroom)
                {
                    powerup.transform.GetChild(0).GetComponent<Animator>().SetTrigger("spawn-mushroom");
                }
                else
                {
                    powerup.GetComponent<Animator>().SetTrigger("startAnim");
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