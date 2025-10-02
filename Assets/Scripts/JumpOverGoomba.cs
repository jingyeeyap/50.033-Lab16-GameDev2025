using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpOverGoomba : MonoBehaviour
{
    public Transform enemyLocation;
    public TextMeshProUGUI scoreText;
    private bool onGroundState;

    [System.NonSerialized]
    public int score = 0; // we don't want this to show up in the inspector
    private bool countScoreState = false;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    private float jumpStartX;
    public PlayerMovement playerMovement;

    public void ResetState()
    {
        score = 0;
        scoreText.text = "Score: 0";
        countScoreState = false;
    }
    void FixedUpdate()
    {
        if (playerMovement.alive)
        {
            // mario jumps
            if (Input.GetKeyDown("space") && onGroundCheck())
            {
                onGroundState = false;
                countScoreState = true;
                jumpStartX = transform.position.x; // record where the jump started
            }

            // when jumping, and Goomba is near Mario and we haven't registered our score
            // if (!onGroundState && countScoreState)
            // {
            //     if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            //     {
            //         countScoreState = false;
            //         score++;
            //         scoreText.text = "Score: " + score.ToString();
            //     }
            // }

            if (onGroundCheck() && countScoreState && onGroundState)
            {
                // check if Mario has crossed the Goomba during this jump
                if ((jumpStartX < enemyLocation.position.x && transform.position.x > enemyLocation.position.x) ||
                    (jumpStartX > enemyLocation.position.x && transform.position.x < enemyLocation.position.x))
                {
                    score++;
                    scoreText.text = "Score: " + score.ToString();
                    countScoreState = false; // reset for next jump
                }
            }

        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }


    private bool onGroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            // Debug.Log("on ground");
            return true;
        }
        else
        {
            // Debug.Log("not on ground");
            return false;
        }
    }

    // helper
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}
