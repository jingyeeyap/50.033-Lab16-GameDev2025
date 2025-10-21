
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireFlowerPowerupWeek5 : BasePowerup
{
    // setup this object's type
    // instantiate variables
    public AudioSource objectAudio, pickUpAudio;
    Collider2D col;
    public UnityEvent<IPowerup> powerupCollected;
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.FireFlower;

        col = GetComponent<Collider2D>();
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
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        StartCoroutine(EnableCollider());
        if (spawned == false) objectAudio.PlayOneShot(objectAudio.clip);
        spawned = true;
        // powerupCollected.Invoke(this);
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        MarioStateController mario;
        bool result = i.TryGetComponent<MarioStateController>(out mario);
        if (result)
        {
            mario.SetPowerup(this.powerupType);
        }
    }

    public void GameRestart()
    {
        spawned = false;
        gameObject.SetActive(true);

        col.enabled = false;
        // Debug.Log("reset pos: " + startingPos);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("gameRestart");
        // transform.GetChild(0).transform.position = Vector3.zero;
    }

    IEnumerator EnableCollider()
    {
        col.enabled = true;
        yield return null;
    }
}