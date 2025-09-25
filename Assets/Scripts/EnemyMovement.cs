using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private String otherGameObjectName;
    public Vector3 startPosition;

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
    
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
            // move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    // Unity's architecture requires many of its core functions, especially those interacting with GameObjects, 
    // components, or the scene hierarchy, to be executed on the main thread for thread safety and consistency.
    void OnTriggerEnter2D(Collider2D other)
    {
        // If an asynchronous operation is triggered here, and its callback attempts to use GetName:
        // Instead of calling GetName directly in the callback, schedule it on the main thread.
        Invoke("CallGetNameOnMainThread", 0f);
        // Debug.Log(otherGameObjectName);
    }

    void CallGetNameOnMainThread()
    {
        // Access GetName or other Unity APIs here
        otherGameObjectName = gameObject.name; // This is safe on the main thread
    }
}