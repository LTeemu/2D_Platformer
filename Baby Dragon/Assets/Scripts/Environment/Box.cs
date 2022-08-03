using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject boxRef;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("PowderKeg"))
        {
            Instantiate(boxRef, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (coll.gameObject.CompareTag("Fireball"))
        {
            Instantiate(boxRef, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
