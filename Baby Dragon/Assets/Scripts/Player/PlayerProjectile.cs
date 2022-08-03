using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetimeMax;
    [SerializeField] int damage;
    private float lifetime;
    private float direction;
    private Rigidbody2D rb;
    [SerializeField] ParticleSystem collisionParticle;
    [SerializeField] ParticleSystem flameParticle;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (lifetime < lifetimeMax)
        {
            lifetime += Time.deltaTime;
        }              
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {       
        rb.AddForce(new Vector3(direction * speed , 0, 0), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.velocity = Vector2.zero;
        flameParticle.Stop();
        flameParticle.Clear();
        collisionParticle.Play();
        Destroy(gameObject, 0.1f);
        if (collision.CompareTag("Enemy")) {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            enemyToCombat(collision);
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;      
        if (Mathf.Sign(transform.localScale.x) != direction)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
        }
    }

    private void Destroy()
    {       
        Destroy(gameObject);
    }

    private void enemyToCombat(Collider2D collider)
    {
        if (collider.name.Contains("Knight"))
        {
            collider.GetComponent<KnightController>().inCombat = true;
        }
        else if (collider.name.Contains("Ranger"))
        {
            collider.GetComponent<RangerController>().inCombat = true;
        }
        else if (collider.name.Contains("Slime"))
        {
            collider.GetComponent<SlimeController>().inCombat = true;
        }
    }
}
