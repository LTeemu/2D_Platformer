using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private float attackCooldownTimer = Mathf.Infinity;
    [SerializeField] LayerMask enemyLayer;
    private Animator animator;
    private PlayerController playerController;
    [Header("Ranged")]
    [SerializeField] private Transform firePoint;
    public PlayerProjectile projectilePrefab;
    bool rangedBtnDown = false;

    [Header("Melee")]
    [SerializeField] float meleeRangeX;
    [SerializeField] float meleeRangeY;
    [SerializeField] private int meleeDamage;
    [SerializeField] private float meleeKnockback;
    [SerializeField] private Transform meleePos;
    bool meleeBtnDown = false;

    private float playerVelocityX;
    public Image[] fireballs;
    public Sprite fullFireball;
    public Sprite emptyFireball;
    [SerializeField] int startFireballCount;
    private int fireballCount;
    [SerializeField] float fireballRegen;
    private float fireballTimer;
    private AudioManager audioManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        fireballCount = startFireballCount;
    }

    void Update()
    {
        //PC NAPIT
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            meleeBtnPress();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            meleeBtnRelease();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            rangedBtnPress();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            rangedBtnRelease();
        }

        if (rangedBtnDown && attackCooldownTimer > attackCooldown && fireballCount > 0)
            RangedAttack();

        if (meleeBtnDown && attackCooldownTimer > attackCooldown)
            MeleeAttack();

        if (attackCooldownTimer < attackCooldown)       
            attackCooldownTimer += Time.deltaTime;

        playerVelocityX = playerController.velocityX;
        UiFireballs();
        regenAmmo();
    }

    private void MeleeAttack()
    {
        audioManager.Play("DragonMelee");
        attackCooldownTimer = 0;
        Collider2D[] hit = Physics2D.OverlapBoxAll(meleePos.position, new Vector2(meleeRangeX, meleeRangeY), 0, enemyLayer);
        if (hit != null)
        {
            foreach (Collider2D collider in hit)
            {
                //Debug.Log("Melee hit " + collider.name);
                Rigidbody2D colliderRB = collider.GetComponent<Rigidbody2D>();
                if(colliderRB)
                {
                    colliderRB.AddForce(new Vector2(transform.localScale.x * meleeKnockback, meleeKnockback), ForceMode2D.Impulse);
                }
                collider.GetComponent<EnemyHealth>().TakeDamage(meleeDamage);
                enemyToCombat(collider);
            }
        }
    }

    private void RangedAttack()
    {
        fireballCount -= 1;
        attackCooldownTimer = 0;
        animator.SetTrigger("Fireball");
        PlayerProjectile createdObject = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        createdObject.GetComponent<PlayerProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        createdObject.GetComponent<Rigidbody2D>().velocity = new Vector2(playerVelocityX * Mathf.Sign(transform.localScale.x), 0);
        audioManager.Play("Fireball");
    }

    public void rangedBtnRelease()
    {
        rangedBtnDown = false;
    }

    public void rangedBtnPress()
    {
        rangedBtnDown = true;
    }

    public void meleeBtnRelease()
    {
        meleeBtnDown = false;
    }

    public void meleeBtnPress()
    {
        meleeBtnDown = true;
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

    void UiFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (i < fireballCount)
            {
                fireballs[i].sprite = fullFireball;
            }
            else
            {
                fireballs[i].sprite = emptyFireball;
            }

            if (i < startFireballCount)
            {
                fireballs[i].enabled = true;
            }
            else
            {
                fireballs[i].enabled = false;
            }
        }
    }

    public void addFireballs(int count)
    {
        int newCount = fireballCount += count;

        if (newCount > startFireballCount)
        {
            fireballCount = startFireballCount;
        } 
        else
        {
            fireballCount = newCount;
        }
    }

    void regenAmmo()
    {
        if (fireballCount < startFireballCount)
            fireballTimer += Time.deltaTime;

        if (fireballTimer >= fireballRegen)
        {
            fireballCount += 1;
            fireballTimer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(meleePos.position, new Vector2(meleeRangeX, meleeRangeY));
    }
}
