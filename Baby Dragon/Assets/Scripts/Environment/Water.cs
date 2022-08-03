using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private GameObject playerRef;
    private PlayerController playerController;
    private float defaultMovSpeed;

    private void Start()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerController = playerRef.GetComponent<PlayerController>();
        defaultMovSpeed = playerController.movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            float halfTheSpeed = defaultMovSpeed / 2;
            playerController.movementSpeed = halfTheSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerController.movementSpeed = defaultMovSpeed;
        }
    }
}
