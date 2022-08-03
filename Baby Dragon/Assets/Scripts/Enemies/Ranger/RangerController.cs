using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    private Transform player;
    private Rigidbody2D rb;
    private RangerAttack rangerAttack;
    public bool inCombat = false;
    [SerializeField] Transform groundCheckFront;
    [SerializeField] Transform groundCheckBack;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float lookDistance;
    private bool groundAhead;
    private bool groundBehind;
    private bool wallAhead;
    private bool Flipped = false;
    private CapsuleCollider2D collider;

    void Start()
    {
        rangerAttack = GetComponent<RangerAttack>();      
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
       Move();              
    }


    void Move()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        //float distToPlayerX = Vector2.Distance(new Vector2(transform.position.x, player.position.y), new Vector2(player.position.x, player.position.y));
        groundAhead = Physics2D.OverlapCircle(groundCheckFront.position, 0.1f, groundLayer);
        groundBehind = Physics2D.OverlapCircle(groundCheckBack.position, 0.1f, groundLayer);

        wallAhead = Physics2D.BoxCast(collider.bounds.center + new Vector3(collider.bounds.size.x * transform.localScale.x / 2, 0, 0), 
            new Vector3(collider.bounds.size.x / 3, collider.bounds.size.y * 0.8f, 0), 0, Vector2.left, 0, groundLayer);

        if (CanSeePlayer())
        {
            inCombat = true;
        }
        else if (!CanSeePlayer() && distToPlayer > 20 || !player.gameObject.activeInHierarchy)
        {
            inCombat = false;
        }

        if(rb.velocity.y > -1 && rb.velocity.y < 1)
        {
            if (inCombat)
            {
                if (transform.position.x < player.position.x && transform.localScale.x < 0 && Flipped)
                {
                    Flip();
                }
                else if (player.position.x < transform.position.x && transform.localScale.x > 0 && !Flipped)
                {
                    Flip();
                }

                if (rangerAttack.attackCooldownTimer >= rangerAttack.attackCooldown && groundAhead)
                {
                    transform.position += new Vector3(transform.localScale.x * movementSpeed * Time.deltaTime, 0, 0);
                } 
                else if (rangerAttack.attackCooldownTimer < rangerAttack.attackCooldown && groundBehind)
                {
                    transform.position += new Vector3(-transform.localScale.x * movementSpeed / 2 * Time.deltaTime, 0, 0);
                }
            }
            else if (!inCombat)
            {
                if (groundAhead && !wallAhead)
                {
                    transform.position += new Vector3(transform.localScale.x * movementSpeed / 1.33f * Time.deltaTime, 0, 0);
                }
                else if (!groundAhead || wallAhead)
                {
                    Flip();
                }
            }
        }
    }

    void Flip()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1.4f, 2, 1);
            Flipped = false;
        }
        else
        {
            transform.localScale = new Vector3(-1.4f, 2, 1);
            Flipped = true;
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + Vector3.right * lookDistance * transform.localScale.x, playerLayer);
        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckFront.position, 0.1f);
        Gizmos.DrawWireSphere(groundCheckBack.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * lookDistance * transform.localScale.x);

        if(collider)
        {
            //Wallahead
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(collider.bounds.center + new Vector3(collider.bounds.size.x * transform.localScale.x / 2, 0, 0), new Vector3(collider.bounds.size.x / 3, collider.bounds.size.y * 0.8f, 0));
        }
    }
}
