using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(gameObject.name.Contains("Coin"))
            {
                //Score+
                //Debug.Log("picked " + gameObject.name);               
            }
            if (gameObject.name.Contains("Heart"))
            {
                player.GetComponent<PlayerHealth>().PlusOne();
            }
            if (gameObject.name.Contains("Flame"))
            {
                player.GetComponent<PlayerAttack>().addFireballs(1);
            }
            Destroy(gameObject);
        }
    }
}
