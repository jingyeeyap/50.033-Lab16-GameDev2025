
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CoinPowerupWeek5 : BasePowerup
{
    // setup this object's type
    public AudioSource objectAudio;
    [System.NonSerialized] public bool playSound = true;
    // GameManager gameManager;
    public UnityEvent<IPowerup> powerupCollected;
    public GameConstants gameConstants;
    // public UnityEvent onIncrementScore;

    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.Coin;
        // gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
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

    public void UpdatePlaySoundBoolean(bool value)
    {
        playSound = value;
    }


    // interface implementation
    public override void SpawnPowerup()
    {
        // rigidBody.constraints = RigidbodyConstraints2D.None;
        // rigidBody.freezeRotation = true;
        // ApplyPowerup(gameManager.player.GetComponent<PlayerMovementWeek5>());
        if (gameConstants.marioAlive)
        {
            powerupCollected.Invoke(this);
            PlayCoinSound();
        }
        // rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }

    public void PlayCoinSound()
    {
        if (spawned == false && playSound)
        {
            objectAudio.PlayOneShot(objectAudio.clip);
            spawned = true;
        }
        // spawned = true;
        // this.GetComponent<Animator>().SetTrigger("startAnim");
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
        // PlayerMovementWeek5 player = i as PlayerMovementWeek5;
        // if (player.alive)
        // {
        //     PlayCoinSound();
        //     // gameManager.IncreaseScore(1);
        //     // onIncrementScore.Invoke();
        // }
    }

    public void GameRestart()
    {
        spawned = false;
    }
}