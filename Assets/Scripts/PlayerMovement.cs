using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D marioBody;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public float deathImpulse = 15;
    // for animation
    public Animator marioAnimator;
    // for audio
    public AudioSource marioAudio;
    // state
    [System.NonSerialized]
    public bool alive = true;
    public Transform gameCamera;
    GameManager gameManager;
    public AudioSource marioDeathAudio;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        marioSprite = GetComponent<SpriteRenderer>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
        }

    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            Vector2 contactNormal = col.contacts[0].normal;
            if (contactNormal.y > 0.5f)     // fix for mario infinite jump
            {
                onGroundState = true;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }

        if (col.gameObject.CompareTag("Enemy") && alive)
        {
            // Check if Mario is above the enemy
            if (col.contacts[0].normal.y > 0.5f) // Mario hit from above
            {
                EnemyMovement enemy = col.gameObject.GetComponent<EnemyMovement>();

                // Stomp the enemy
                enemy.Stomped();

                // Add small bounce when stomping
                marioBody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

                // Increase score through GameManager
                gameManager.IncreaseScore(1);
            }
            else // Mario hit from side
            {
                // play death animation
                marioAnimator.Play("mario-die");
                marioAudio.PlayOneShot(marioDeathAudio.clip);
                col.collider.enabled = false;
                alive = false;
            }
        }
    }

    private bool moving = false;
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    private bool jumpedState = false;

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            PlayJumpSound();
            jumpedState = false;

        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Enemy") && alive)
    //     {
    //         // play death animation
    //         marioAnimator.Play("mario-die");
    //         marioAudio.PlayOneShot(marioDeathAudio.clip);
    //         alive = false;
    //     }
    // }

    public void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset animation
        marioAnimator.SetTrigger("gameRestart");

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

        alive = true;
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
        // if (marioAudio.isPlaying) Debug.Log("currently playing: " + marioAudio.clip.name);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        gameManager.GameOver();
        // stop time
        // Time.timeScale = 0.0f;
        // // set gameover scene
        // GameOverScreen.SetActive(true);     // set the Game over screen to be active

    }

}