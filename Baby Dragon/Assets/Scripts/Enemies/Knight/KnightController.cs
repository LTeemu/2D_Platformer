using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed;
    private float minDistance = 1f;
    private Transform player;
    private Rigidbody2D rb;
    public bool inCombat = false;
    private bool canMove;

    [Header("Collision")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] bool groundAhead;
    [SerializeField] bool wallAhead;
    [SerializeField] float lookDistance;
    private bool Flipped = false;
    private CapsuleCollider2D collider;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {     
        if(canMove)
        {
            Move();
        }
    }

    private void Update()
    {
        canMove = GetComponent<KnightAttack>().canMove;
    }

    void Move()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        float distToPlayerX = Vector2.Distance(new Vector2(transform.position.x, player.position.y), new Vector2(player.position.x, player.position.y));

        groundAhead = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        wallAhead = Physics2D.BoxCast(collider.bounds.center + new Vector3(collider.bounds.size.x * -transform.localScale.x / 2, 0, 0),
            new Vector3(collider.bounds.size.x / 3, collider.bounds.size.y * 0.9f, 0), 0, Vector2.left, 0, groundLayer);

        if (CanSeePlayer())
        {
            inCombat = true;
        } else if (!CanSeePlayer() && distToPlayer > 30 || !player.gameObject.activeInHierarchy)
        {
            inCombat = false;
        }

        if(rb.velocity.y > -1 && rb.velocity.y < 1) 
        {
            if (inCombat)
            {
                if(transform.position.x < player.position.x && !Flipped || player.position.x < transform.position.x && Flipped)
                {
                    Flip();                    
                } 
                if(distToPlayerX > minDistance && groundAhead && !wallAhead)
                {
                    animator.SetBool("Walking", true);
                    transform.position += new Vector3(-transform.localScale.x * movementSpeed * Time.deltaTime, 0, 0);
                } else
                {
                    animator.SetBool("Walking", false);
                }
            }
            else if (!inCombat)
            {                
                if (groundAhead && !wallAhead)
                {
                    animator.SetBool("Walking", true);
                    transform.position += new Vector3(-transform.localScale.x * movementSpeed / 1.33f * Time.deltaTime, 0, 0);
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

    bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + Vector3.right * lookDistance * -transform.localScale.x, playerLayer);
        return hit;
    }

    private void OnDrawGizmos()
    {
        //Groudcheck
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        
        if(collider)
        {
            //CanSeePlayer
            Gizmos.DrawLine(collider.bounds.center, collider.bounds.center + Vector3.right * lookDistance * -transform.localScale.x);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(collider.bounds.center, collider.bounds.center + Vector3.right * -transform.localScale.x);

            //Wallahead
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(collider.bounds.center + new Vector3(collider.bounds.size.x * -transform.localScale.x / 2, 0, 0), new Vector3(collider.bounds.size.x / 3, collider.bounds.size.y * 0.9f, 0));       
        } 
    }

    void disableController()
    {
        gameObject.GetComponent<KnightController>().enabled = false;
        gameObject.GetComponent<KnightAttack>().enabled = false;
    }
}