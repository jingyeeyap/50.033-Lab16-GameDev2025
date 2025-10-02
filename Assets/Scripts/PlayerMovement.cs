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
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    public float deathImpulse = 15;
    public GameObject GameOverScreen;
    // for animation
    public Animator marioAnimator;
    // for audio
    public AudioSource marioAudio;
    public AudioClip marioDeath;
    // state
    [System.NonSerialized]
    public bool alive = true;
    public Transform gameCamera;
    [System.NonSerialized] public bool restartQuestionBox = false;

    // Start is called before the first frame update
    void Start()
    {
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
        // if (!GameOverScreen.activeInHierarchy)
        // {

        // }

        if (alive)
        {
            // toggle state
            if ((Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow)) && faceRightState)
            {
                faceRightState = false;
                marioSprite.flipX = true;

                if (marioBody.linearVelocity.x > 0.1f)
                    marioAnimator.SetTrigger("onSkid");
            }

            if ((Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow)) && !faceRightState)
            {
                faceRightState = true;
                marioSprite.flipX = false;

                if (marioBody.linearVelocity.x < -0.1f)
                    marioAnimator.SetTrigger("onSkid");
            }

            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
        }

    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (alive)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                // check if it doesn't go beyond maxSpeed
                if (marioBody.linearVelocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
            }

            // stop
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                // stop
                marioBody.linearVelocity = Vector2.zero;
            }

            if (Input.GetKeyDown("space") && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;

                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            // play death animation
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    public void RestartButtonCallback(int input)
    {
        // resume time
        Time.timeScale = 1.0f;
        // reset everything
        ResetGame();
        EventSystem.current.SetSelectedGameObject(null);
        alive = true;

    }

    private void ResetGame()
    {
        restartQuestionBox = true;
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

        if (GameOverScreen.activeInHierarchy) GameOverScreen.SetActive(false);

        // reset animation
        marioAnimator.SetTrigger("gameRestart");

        ResetAllAnimators();

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

        // reset score
        jumpOverGoomba.ResetState();
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
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        GameOverScreen.SetActive(true);     // set the Game over screen to be active
    }

    public Transform obstacles; // assign in Inspector (e.g. root object)
    void ResetAllAnimators()
    {
        if (obstacles == null)
        {
            Debug.LogWarning("No parent assigned!");
            return;
        }

        Animator[] animators = obstacles.GetComponentsInChildren<Animator>(true);
        // (true) includes disabled objects too

        foreach (Animator anim in animators)
        {
            if (anim.gameObject.CompareTag("QuestionBox"))
            {
                anim.SetTrigger("gameRestart");
                // clear it immediately so it won’t interfere with other transitions
                anim.ResetTrigger("marioHit");
            }
        }
    }

}