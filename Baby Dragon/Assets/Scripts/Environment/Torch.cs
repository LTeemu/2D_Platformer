using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private GameObject torchRef;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Fireball"))
        {
            Instantiate(torchRef, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}

