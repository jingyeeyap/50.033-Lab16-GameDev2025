
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinPowerup : BasePowerup
{
    // setup this object's type
    public AudioSource objectAudio;
    [System.NonSerialized] public bool playSound = true;
    GameManager gameManager;
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.Coin;
        GameManager.instance.gameRestart.AddListener(GameRestart);
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            // then destroy powerup (optional)
            // DestroyPowerup();

        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        // rigidBody.constraints = RigidbodyConstraints2D.None;
        // rigidBody.freezeRotation = true;
        PlayCoinSound();

        // rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }

    public void PlayCoinSound()
    {
        if (spawned == false && playSound)
        {
            objectAudio.PlayOneShot(objectAudio.clip);
            spawned = true;
            gameManager.IncreaseScore(1);
        }
        // spawned = true;
        // this.GetComponent<Animator>().SetTrigger("startAnim");
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
    }

    public void GameRestart()
    {
        spawned = false;
    }
}