
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StarPowerup : BasePowerup
{
    // setup this object's type
    // instantiate variables
    public AudioSource objectAudio, pickUpAudio;
    Vector2 startingPos;
    Collider2D col;
    public UnityEvent<IPowerup> powerupCollected;
    SpriteRenderer spriteRenderer;
    protected override void Start()
    {
        base.Start(); // call base class Start()
        startingPos = transform.localPosition;

        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            // ApplyPowerup(col.gameObject.GetComponent<PlayerMovement>());
            ApplyPowerup(col.gameObject.GetComponent<MonoBehaviour>());
            powerupCollected.Invoke(this);
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
        // powerupCollected.Invoke(this);

        int enemyLayer = LayerMask.NameToLayer("Enemies");
        int powerupLayer = gameObject.layer;
        Physics2D.IgnoreLayerCollision(powerupLayer, enemyLayer, true);

        StartCoroutine(AutoDestroyAfterDelay(10f, 2f));
    }

    private IEnumerator AutoDestroyAfterDelay(float totalTime, float blinkDuration)
    {
        float blinkStartTime = totalTime - blinkDuration;

        // Wait until blink phase starts
        yield return new WaitForSeconds(blinkStartTime);

        float elapsed = 0f;
        while (elapsed < blinkDuration && spawned)
        {
            float normalized = elapsed / blinkDuration;
            float blinkInterval = Mathf.Lerp(0.2f, 0.05f, normalized);

            if (spriteRenderer)
                spriteRenderer.enabled = !spriteRenderer.enabled; // toggle visibility

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // Ensure sprite visible before destruction
        if (spriteRenderer)
            spriteRenderer.enabled = true;

        if (spawned)
            DestroyPowerup();
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        BuffStateController buff;
        bool buffResult = i.TryGetComponent<BuffStateController>(out buff);
        if (buffResult)
        {
            buff.SetPowerup(this.powerupType);
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