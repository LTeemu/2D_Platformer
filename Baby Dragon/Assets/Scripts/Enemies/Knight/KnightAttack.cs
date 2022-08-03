using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown;
    [HideInInspector] public float attackCooldownTimer = Mathf.Infinity;
    public float attackRange;
    [SerializeField] int meleeDamage;
    [SerializeField] float knockback;
    [SerializeField] float attackRangeY;
    [SerializeField] float attackRangeX;
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;

    [Header("Collision")]
    [SerializeField] LayerMask playerLayer;
    private CapsuleCollider2D collider;

    [Header("References")]
    private GameObject player;
    private Animator animator;
    private AudioManager audioManager;

    [Header("Movement")]
    public bool canMove = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (attackCooldownTimer < attackCooldown)
            attackCooldownTimer += Time.deltaTime;
        CheckAttackArea();
    }

    private void CheckAttackArea()
    {
        if(collider)
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(collider.bounds.center + new Vector3(attackRangeX * -transform.localScale.x * offsetX, -transform.localScale.y * offsetY, 0),
                new Vector3(collider.bounds.size.x * attackRangeX, collider.bounds.size.y * attackRangeY, collider.bounds.size.z),
                0, Vector2.left, 0, playerLayer);

            if (rayHit.collider != null && rayHit.transform.CompareTag("Player") && attackCooldownTimer > attackCooldown && canMove)
            {
                animator.SetTrigger("Attack");               
                canMove = false;                
            }
        }
    }

    void MeleeAttack()
    {       
        if (collider)
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(collider.bounds.center + new Vector3(attackRangeX * -transform.localScale.x * offsetX, -transform.localScale.y * offsetY, 0),
            new Vector3(collider.bounds.size.x * attackRangeX, collider.bounds.size.y * attackRangeY, collider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

            attackCooldownTimer = 0;
            if (!player.GetComponent<PlayerHealth>().invulnerable && rayHit.collider != null)
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * knockback, knockback), ForceMode2D.Impulse);
                player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
            }
        }
    }

    void CanMoveTrue()
    {
        canMove = true;
    }

    void PlayDeathSound()
    {
        audioManager.Play("KnightDead");
    }

    private void OnDrawGizmos()
    {
        if(collider)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(collider.bounds.center + new Vector3(attackRangeX * -transform.localScale.x * offsetX, -transform.localScale.y * offsetY, 0),
            new Vector3(collider.bounds.size.x * attackRangeX, collider.bounds.size.y * attackRangeY, collider.bounds.size.z));
        }
    }
    
    void playSwordSwing()
    {
        audioManager.Play("SwordSwing");
    }
}
