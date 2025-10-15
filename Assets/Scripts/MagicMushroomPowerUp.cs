
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    // setup this object's type
    // instantiate variables
    public AudioSource objectAudio, pickUpAudio;
    Vector2 startingPos;

    Collider2D col;
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;
        GameManager.instance.gameRestart.AddListener(GameRestart);
        startingPos = transform.localPosition;

        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        // Continuously move if spawned and dynamic
        if (spawned && rigidBody.bodyType == RigidbodyType2D.Dynamic)
        {
            float desiredX = (goRight ? 1 : -1) * 3f;
            rigidBody.linearVelocity = new Vector2(desiredX, rigidBody.linearVelocityY);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            ApplyPowerup(col.gameObject.GetComponent<PlayerMovement>());
            // then destroy powerup (optional)
            DestroyPowerup();

        }
        else if (col.gameObject.layer == 7) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                // rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);
            }
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        StartCoroutine(EnableRbAndCollider());
        if (spawned == false) objectAudio.PlayOneShot(objectAudio.clip);
        spawned = true;

    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
        PlayerMovement player = i as PlayerMovement;
        if (player != null)
        {
            // Start invincibility
            if (player.isInvincible == false)
            {
                pickUpAudio.PlayOneShot(pickUpAudio.clip);
                player.StartCoroutine(player.Invincibility(5f));
            }
        }
    }

    public void GameRestart()
    {
        spawned = false;
        gameObject.SetActive(true);

        col.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;
        // Debug.Log("reset pos: " + startingPos);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("gameRestart");
        transform.localPosition = startingPos;
        // transform.GetChild(0).transform.position = Vector3.zero;
    }

    IEnumerator EnableRbAndCollider()
    {
        col.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        yield return null;

        goRight = true; // ensure this is reset just before starting motion
        rigidBody.linearVelocity = new Vector2(3f, rigidBody.linearVelocityY);
    }
}