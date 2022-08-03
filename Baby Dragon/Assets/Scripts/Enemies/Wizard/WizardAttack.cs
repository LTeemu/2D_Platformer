using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown;
    [HideInInspector] public float attackCooldownTimer = Mathf.Infinity;
    public float attackRange;
    public Transform firePoint;
    private Transform player;
    public WizardIceProjectile projectilePrefab;
    public float projectileForce;  
    AudioManager audioManager;
    Vector3 attackDirection;

    [Header("Collision")]
    [SerializeField] LayerMask playerMask;

    [Header("References")]
    private Animator animator;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    void Update()
    {
        if (attackCooldownTimer < attackCooldown)
            attackCooldownTimer += Time.deltaTime;

        CheckAttackArea();
    }

    // Check is Wizard have LOS to player
    private void CheckAttackArea()
    {
        float distToPlayer;
        distToPlayer = Vector2.Distance(transform.position, player.position);
        //Debug.Log("Distance to Player is " + distToPlayer);
        Vector3 drawDirection = (player.position - firePoint.position);
        RaycastHit2D rayHit = Physics2D.Raycast(firePoint.position, drawDirection, attackRange, playerMask);
        //Debug.DrawRay(firePoint.position, drawDirection, Color.green);
        
        if (rayHit.collider != null && rayHit.transform.CompareTag("Player") && attackCooldownTimer > attackCooldown && distToPlayer <= attackRange)
        {
            animator.SetBool("Floating", false);
            animator.SetBool("Preparing", true);

            ChargeAttack();
        }
        else if (distToPlayer >= attackRange)
        {
            animator.SetBool("Preparing", false);
            animator.SetBool("Attacking", false);
            animator.SetBool("Floating", true);
        }  
    }

    // Starts Attacking-animation
    void ChargeAttack()
    {
        animator.SetBool("Attacking", true);
    }

    // Fires WizardIceProjectile during wizard_spell_ice_climax - animation
    void FireIceProjectile()
    {
        attackDirection = (player.position - transform.position).normalized;
        attackCooldownTimer = 0f;
        if (player.position.x < transform.position.x)
        {
            WizardIceProjectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(attackDirection* projectileForce);
            PlayIceCreated();
        }
        else
        {
            WizardIceProjectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.transform.Rotate(0.0f, 180.0f, 0.0f);
            projectile.GetComponent<Rigidbody2D>().AddForce(attackDirection * projectileForce);
            PlayIceCreated();
        }
        //Debug.Log("WIZARD ATTACKS!");
    }

    void PlayIceCreated()
    {
        audioManager.Play("IceProjectileCreated");
    }
}
