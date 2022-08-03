using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardIceProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetimeMax;
    private float lifetime;
    private float direction;
    [SerializeField] int damage;
    public float projectileRotation;
    WizardAttack wizardAttack;

    AudioManager audioManager;

    [Header("References")]
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        CheckProjectileLifeTime();
        HomingProjectile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Ranger hit " + collision.gameObject.name);
        Destroy(gameObject);
        StopIceCreated();
        PlayIceDestroy();
        //Debug.Log("GAMEOBJECT DESTROYED!");
        if (collision.CompareTag("Player"))
        {
            //collision.gameObject.SetActive(false);
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void CheckProjectileLifeTime()
    {
        if (lifetime < lifetimeMax)
        {
            lifetime += Time.deltaTime;
        }
        else
        {
            //StopIceCreated();
            Destroy(gameObject);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    void PlayIceDestroy()
    {
        audioManager.Play("IceProjectileDestroyed");
    }

    void StopIceCreated()
    {
        this.audioManager.Stop("IceProjectileCreated");
    }

    // Makes WizardIceProjectile follow the player
    void HomingProjectile()
    { 
        float rotate = projectileRotation * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, rotate);
        //Debug.DrawRay(transform.position, attackDirection, Color.red);
    }
}
