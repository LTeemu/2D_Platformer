using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKegExpl : MonoBehaviour
{
    [SerializeField] private int explosionDamage = 3;
    private Rigidbody2D playerRb;
    private AudioManager audioManager;

    private void Start()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("PowderKegExpl");
        Destroy(this.gameObject, 0.6f);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            //This is to prevent the player from jumping really high during the explosion
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
            coll.GetComponent<PlayerHealth>().TakeDamage(explosionDamage);
        }
        else if (coll.gameObject.CompareTag("Enemy"))
        {
            coll.GetComponent<EnemyHealth>().TakeDamage(explosionDamage);
        }
    }
}
