using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown;
    [HideInInspector] public float attackCooldownTimer = Mathf.Infinity;
    public float attackRangeX;    
    public Transform firePoint;
    public RangerProjectile projectilePrefab;

    [Header("Collision")]
    [SerializeField] LayerMask playerMask;

    void Update()
    {
        if (attackCooldownTimer < attackCooldown)
            attackCooldownTimer += Time.deltaTime;

        checkAttackArea();
    }

    private void checkAttackArea()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(firePoint.position, new Vector2(transform.localScale.x, 0), attackRangeX, playerMask);
        //Debug.DrawRay(firePoint.position, new Vector2(transform.localScale.x * attackRangeX, 0), Color.green);
        if (rayHit.collider != null && rayHit.transform.CompareTag("Player") && attackCooldownTimer > attackCooldown)
        {
            //Debug.Log(rayHit.collider.gameObject.name);
            RangedAttack();           
        }       
    }

    void RangedAttack()
    {
        attackCooldownTimer = 0;
        RangerProjectile createdObject = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        createdObject.GetComponent<RangerProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firePoint.position, new Vector2(transform.localScale.x * attackRangeX, 0));
    }
}
