using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed;
    private Transform player;
    private Rigidbody2D rb;
    [SerializeField] bool canMove;

    [SerializeField] bool charging = false;
    private bool Flipped = false;

    [Header("Collision")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;

    [Header("Attack")]
    [SerializeField] int meleeDamage;
    [SerializeField] float knockback;      
    [SerializeField] float meleeCooldown;
    private float meleeCooldownTimer;
    [SerializeField] float specialCooldown;
    [SerializeField] float specialCooldownTimer;
    private Vector3 targetPos;
    [SerializeField] float attackRangeY;
    [SerializeField] float attackRangeX;
    [SerializeField] float offSetX;
    [SerializeField] float offSetY;
    [SerializeField] float invokeTime;
    private bool halfHP = false;
    
    [Header("References")]
    private Animator animator;
    private CapsuleCollider2D collider;
    private int startHP;
    [SerializeField] AudioManager audioManager;
    [SerializeField] ParticleSystem particleSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        startHP = GetComponent<EnemyHealth>().startHealth;
    }

    private void Update()
    {
        if (meleeCooldownTimer < meleeCooldown)
            meleeCooldownTimer += Time.deltaTime;
        if (specialCooldownTimer < specialCooldown && !charging)
            specialCooldownTimer += Time.deltaTime;

        if (!halfHP)
        {
            int currentHP = GetComponent<EnemyHealth>().health;
            if(startHP / 2 >= currentHP)
            {               
                invokeTime /= 2;
                specialCooldown /= 2;
                movementSpeed *= 1.33f;
                halfHP = true;
            }
        }

        //Flipping
        if (canMove && (transform.position.x < player.position.x && !Flipped || player.position.x < transform.position.x && Flipped))
        {
            Flip();
        }

        //Start charge attack
        float distToPlayerX = Vector2.Distance(new Vector2(transform.position.x, player.position.y), new Vector2(player.position.x, player.position.y));
        if (specialCooldownTimer >= specialCooldown && canMove)
        {
            animator.SetBool("Walking", false);
            canMove = false;
            audioManager.Play("SpecialAttack");
            if (distToPlayerX > 5)
            {
                //Debug.Log("Target pos: " + targetPos + " Player: " + player.position);
                LockTargetPos();
                Invoke("ChargeAttack", invokeTime);
            }
            else
            {
                Invoke("DoubleSwing", invokeTime);
            }          
        }

        //Charge attack position check
        if (charging)
        {
            float distToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(targetPos.x, transform.position.y));
            if (distToTarget < 0.3f)
            {
                charging = false;
                canMove = true;
                specialCooldownTimer = 0;
                Physics2D.IgnoreLayerCollision(8, 9, true);
            }
        }
    }

    void FixedUpdate()
    {
        //Moving
        if (canMove)
        {
            Move();
        }

        //Charge attack movement
        if(charging)
        {
            Vector3 direction = transform.position - targetPos;
            //Debug.Log("Boss pos: " + transform.position + " target: " + targetPos);
            if (transform.position != new Vector3(targetPos.x, transform.position.y, 0))
            {
                transform.position += new Vector3(-direction.x * movementSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    void Move()
    {
        float distToPlayerX = Vector2.Distance(new Vector2(transform.position.x, player.position.y), new Vector2(player.position.x, player.position.y));        
        if(distToPlayerX > attackRangeX)
        {            
            transform.position += new Vector3(-transform.localScale.x * movementSpeed * Time.deltaTime, 0, 0);
            animator.SetBool("Walking", true);
        } 
        else
        {
            animator.SetBool("Walking", false);
        }
        
        if (meleeCooldownTimer >= meleeCooldown && specialCooldownTimer < specialCooldown && distToPlayerX <= attackRangeX && canMove)
        {           
            animator.SetTrigger("Attack");
            canMove = false;
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

    void MeleeAttack()
    {     
        meleeCooldownTimer = 0;

        if (collider)
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(collider.bounds.center + new Vector3(attackRangeX * -transform.localScale.x * offSetX, -transform.localScale.y * offSetY, 0),
            new Vector3(collider.bounds.size.x * attackRangeX, collider.bounds.size.y * attackRangeY, collider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

            if (!player.GetComponent<PlayerHealth>().invulnerable && rayHit.collider != null)
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * knockback, knockback), ForceMode2D.Impulse);
                player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
            }
        }
    }

    void LockTargetPos()
    {       
        targetPos = new Vector3(player.position.x, transform.position.y, 0);
        if (transform.position.x < targetPos.x && !Flipped || targetPos.x < transform.position.x && Flipped)
        {
            Flip();
        }
    }

    void DoubleSwing()
    {
        TriggerAttack();
        Invoke("Flip", 0.5f);
        Invoke("TriggerAttack", 0.5f);
        ResetCooldowns();
    }

    void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    void ChargeAttack()
    {  
        charging = true;
        //Turn on player / enemy collision
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * knockback, knockback), ForceMode2D.Impulse);
            player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);

            charging = false;
            canMove = true;
            specialCooldownTimer = 0;
            Physics2D.IgnoreLayerCollision(8, 9, true);
        }
    }

    void ResetCooldowns()
    {
        specialCooldownTimer = 0;
        meleeCooldownTimer = 0;
    }

    void CanMoveTrue()
    {
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        //MeleeAttack
        if (collider)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(collider.bounds.center + new Vector3(attackRangeX * -transform.localScale.x * offSetX,  -transform.localScale.y * offSetY, 0),
            new Vector3(collider.bounds.size.x * attackRangeX, collider.bounds.size.y * attackRangeY, collider.bounds.size.z));
        }
    }

    void playParticle()
    {
        particleSystem.Play();
    }

    void disableController()
    {
        gameObject.GetComponent<ForestBossController>().enabled = false;
    }

    void playSwordSwing()
    {
        audioManager.Play("SwordSwing");
    }
}