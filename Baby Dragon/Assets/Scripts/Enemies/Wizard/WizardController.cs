using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float optimalDistance;
    [SerializeField] float agroRange;
    
    private Transform player;
    private Rigidbody2D enemyRB;
    private SpriteRenderer spriteRenderer;
    private WizardAttack wizardAttack;
    private CapsuleCollider2D wizardCollider;

    private Animator animator;

    void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        wizardCollider = GetComponent<CapsuleCollider2D>();
        //spriteRenderer.color = Random.ColorHSV();
        wizardAttack = GetComponent<WizardAttack>();
        //minDistance = Random.Range(-wizardAttack.attackRangeX, wizardAttack.attackRangeX);
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
            return;

        Flip();
        MoveTowardsPlayer();
        CheckOverlap();
    }

    public void MoveTowardsPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        //float distToPlayerX = Vector2.Distance(new Vector2(transform.position.x, player.position.y), new Vector2(player.position.x, player.position.y));
        float advance = movementSpeed * Time.deltaTime;
        //Debug.Log("Distance to Player is " + distToPlayer);

        // moves closer to player
        if (distToPlayer <= optimalDistance - 0.1)
        {
            transform.position += new Vector3(1, 1, 0) * movementSpeed * Time.deltaTime;
        }
        // moves away from player
        else if (distToPlayer >= optimalDistance + 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, advance);
        }
        // stops chasing player
        else if (distToPlayer > agroRange)
        {
            animator.SetBool("Floating", true);
        }
    }

    void Flip()
    {
        // if player is on the left of the wizard it keeps it's original facing
        if (transform.position.x > player.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        // if player is on the right of the wizard it flips 180 degrees
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // This checks that the wizards doesn't stack.
    void CheckOverlap()
    {
        // speed of moving when overlapping
        float separateSpeed = movementSpeed / 2f;
        // distance how far wizard is getting from another when they overlap
        float personalSpace = 2f;

        Vector2 sum = Vector2.zero;
        float count = 0f;

        // detects overlapping
        var hits = Physics2D.OverlapCircleAll(transform.position, personalSpace);

        foreach (var hit in hits)
        { // checks that it's colliding with another wizard
            if (hit.gameObject.GetComponent<Rigidbody2D>() != null && hit.transform != transform)
            {
                // gets difference so it knows which way to go
                Vector2 difference = transform.position - hit.transform.position;

                // the closer the wizard to another the more it moves
                difference = difference.normalized / Mathf.Abs(difference.magnitude);

                // gets the average of the group overlapping and moves enemies in the edges more than in the center of the group
                sum += difference;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized * separateSpeed;
            // applying vector where to move and how much and at what speed
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)sum, separateSpeed * Time.fixedDeltaTime);
        }
    }
}   
