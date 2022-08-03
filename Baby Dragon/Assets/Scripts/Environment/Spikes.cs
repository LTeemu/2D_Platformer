using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int spikesDamage = 1;
    [SerializeField] private int knockForce = 10;
    private Rigidbody2D playerRb;

    private void Start()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            //This is to prevent the player from jumping really high during the knockback
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
            playerRb.AddForce(Vector2.up * knockForce, ForceMode2D.Impulse);
            coll.gameObject.GetComponent<PlayerHealth>().TakeDamage(spikesDamage);
        } 
    }
}
