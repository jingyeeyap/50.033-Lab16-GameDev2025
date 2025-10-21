using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovementWeek5 : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    public int moveSpeed = 10;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    Vector3 startPosition;
    bool isAlive = true;
    public Animator enemyAnimator;
    public UnityEvent<int> onIncrementScore;
    public UnityEvent damagePlayer;
    public GameConstants gameConstants;

    void Awake()
    {
        startPosition = transform.localPosition;
    }

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();

    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            enemyBody.linearVelocity = new Vector2(moveRight * moveSpeed, enemyBody.linearVelocity.y);
        }
        else
        {
            enemyBody.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive || !gameConstants.marioAlive) return;

        if (collision.gameObject.layer == 7)
        {
            // Flip direction
            moveRight *= -1;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (moveRight == 1 ? 1 : -1);
            transform.localScale = scale;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.contacts[0];

            // Check if contact came from above (normal pointing downward)
            bool hitFromAbove = contact.normal.y < -0.5f;

            if (hitFromAbove)
            {
                Stomped();
                Rigidbody2D marioBody = collision.gameObject.GetComponent<Rigidbody2D>();
                marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, 15f);
            }
            else
            {
                damagePlayer.Invoke();
            }
        }

        if (collision.gameObject.CompareTag("Fireball"))
        {
            enemyAnimator.SetTrigger("goomba-killed-fireball");
            AfterDeathEvents();
        }

        // If Mario stomps from above, handled elsewhere
    }

    public void GameRestart()
    {
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.GetComponent<Collider2D>().enabled = true;
        isAlive = true;

        enemyAnimator.SetTrigger("gameRestart");
    }

    public void Stomped()
    {
        enemyAnimator.SetTrigger("goomba-killed");

        AfterDeathEvents();
    }

    public void AfterDeathEvents()
    {
        // Disable collision so Mario can pass through
        if (this.GetComponent<Collider2D>() != null)
        {
            this.GetComponent<Collider2D>().enabled = false;
        }

        isAlive = false;
        onIncrementScore.Invoke(1);
    }

}