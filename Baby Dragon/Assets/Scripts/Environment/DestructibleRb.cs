using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRb : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 forceDirection;
    [SerializeField] private float torque;
    [SerializeField] private float delayBeforeGone;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float randTorque = UnityEngine.Random.Range(-30, 120);
        float randForceX = UnityEngine.Random.Range(forceDirection.x - 50, forceDirection.x + 50);
        float randForceY = UnityEngine.Random.Range(forceDirection.y, forceDirection.x + 50);

        forceDirection.x = randForceX;
        forceDirection.y = randForceY;

        rb.AddForce(forceDirection);
        rb.AddTorque(randTorque);
    }

    private void Update()
    {
        Destroy(this.gameObject, delayBeforeGone);
    }
}
