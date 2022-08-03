using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float bounceX;
    [SerializeField] float bounceY;
    [SerializeField] float bounceCooldown;
    private float bounceCooldownTimer;
    private Transform player;
    private Rigidbody2D rb;
    public bool inCombat = false;

    [Header("Collision")]
    private BoxCollider2D collider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] bool isGrounded;
    private bool wallAhead;
    [SerializeField] float lookDistance;
    [SerializeField] bool Flipped = false;

    private Animator animator;
    [SerializeField] int damage;
    private float knockback;
    private AudioManager audioManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        bounceCooldownTimer = bounceCooldown;
        collider = GetComponent<BoxCollider2D>();
        audioManager = FindObjectOfType<AudioManager>();

        //Random values
        float scale = Random.Range(0.4f, 1.5f);
        gameObject.GetComponent<EnemyHealth>().SetHealth(Mathf.RoundToInt(4f * scale));
        transform.localScale = new Vector3(scale, scale, 1);
        bounceX = 5f / scale;
        bounceY = bounceX;
        bounceCooldown = 1f * scale;
        knockback = 1f + (scale * 5f);
    }

    void FixedUpdate()
    {
        Move();        
    }


    void Move()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
       
        isGrounded = Physics2D.BoxCast(collider.bounds.center - new Vector3(0, collider.bounds.size.y / 2f), new Vector3(collider.bounds.size.x, 0.1f), 0f, Vector2.down, 0.05f, groundLayer);

        //Wallahead
        if(transform.localScale.x >= 1f)
        {
            wallAhead = Physics2D.BoxCast(collider.bounds.center + new Vector3(collider.bounds.size.x * -transform.localScale.x / 2f, 0, 0),
            new Vector3(collider.bounds.size.x / 2f, collider.bounds.size.y * 0.9f, 0), 0, Vector2.left, 0, groundLayer);
        } 
        else
        {
            wallAhead = Physics2D.BoxCast(collider.bounds.center + new Vector3(collider.bounds.size.x * Mathf.Sign(-transform.localScale.x) / 2f, 0, 0),
            new Vector3(1f - transform.localScale.x + collider.bounds.size.x / 2f, collider.bounds.size.y * 0.9f, 0), 0, Vector2.left, 0, groundLayer);
        }

        //Animator
        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
        } 
        else
        {
            animator.SetBool("isGrounded", false);
        }

        if (CanSeePlayer())
        {
            inCombat = true;
        } else if (!CanSeePlayer() && distToPlayer > 20 || !player.gameObject.activeInHierarchy)
        {
            inCombat = false;
        }

            if (inCombat)
            {
                if(transform.position.x < player.position.x && !Flipped)
                {
                    Flip();                    
                } 
                else if (player.position.x < transform.position.x && Flipped)
                {
                    Flip();
                }
                if(isGrounded)
                {
                    Bounce(bounceCooldown / 2);
                }
            }
            else if (!inCombat)
            {
                if (isGrounded)
                {
                    Bounce(bounceCooldown);
                }
                else if (wallAhead && rb.velocity.y <= 0)
                {
                    Flip();
                }
            }
              
    }

    void Flip()
    {
        if (Flipped)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1);
            Flipped = false;
        }
        else
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1);
            Flipped = true;
        }
    }

    void Bounce(float cooldown)
    {

            if(bounceCooldownTimer <= 0)
            {
            //Slime bounce audio
                audioManager.Play("SlimeBounce");
                animator.SetTrigger("Jumping");
                rb.AddForceAtPosition(new Vector2(-transform.localScale.x * bounceX, bounceY), transform.position, ForceMode2D.Impulse);
                bounceCooldownTimer = cooldown;
            }
            else
            {
                bounceCooldownTimer -= Time.deltaTime;
            }
        

        Collider2D col = Physics2D.OverlapBox(collider.bounds.center, collider.bounds.size, 0f, playerLayer);        
        if (col)
        {
            if (!player.GetComponent<PlayerHealth>().invulnerable)
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * knockback, knockback), ForceMode2D.Impulse);
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Linecast(collider.bounds.center, collider.bounds.center + Vector3.right * lookDistance * -transform.localScale.x, playerLayer);
        return hit;
    }

    void DisableMovement()
    {
        gameObject.GetComponent<SlimeController>().enabled = false;
    }
   
    private void OnDrawGizmos()
    {
        if(collider)
        {
            //Groudcheck
            Gizmos.DrawWireCube(collider.bounds.center - new Vector3(0, collider.bounds.size.y / 2), new Vector3(collider.bounds.size.x, 0.1f));
            //CanSeePlayer
            Gizmos.DrawLine(collider.bounds.center, collider.bounds.center + Vector3.right * lookDistance * -transform.localScale.x);
            //AttackBox
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
            //Wallahead
            
            if (transform.localScale.x >= 1f)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(collider.bounds.center + new Vector3(collider.bounds.size.x * -transform.localScale.x / 2, 0, 0), 
                    new Vector3(collider.bounds.size.x / 2f, collider.bounds.size.y * 0.9f, 0));
            }
            else
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(collider.bounds.center + new Vector3(collider.bounds.size.x * Mathf.Sign(-transform.localScale.x) / 2, 0, 0), 
                    new Vector3(collider.bounds.size.x / 2f, collider.bounds.size.y * 0.9f, 0));
            }
        }
    }
}