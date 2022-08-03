using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int startHealth;
    public int health;
    public DropTable dropTable;
    private bool droppedLoot = false;
    private Animator animator;
    private AudioManager audioManager;

    private void Start()
    {
        SetStartHealth();
        
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (health <= 0)
        {          
            dropLoot();
            Die();          
        }
    }

    public void TakeDamage(int damage)
    {
        SetHealth(health - damage);
        animHurt();
        if(gameObject.name.Contains("KnightEnemy"))
        {
            audioManager.Play("KnightHit");
        }
    }

    public void ResetPlayer()
    {
        health = startHealth;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    void animHurt()
    {
        if (animator)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name.Contains("Hurt"))
                {
                    animator.SetTrigger("Hurt");
                }
            }
        }
    }

    void Die()
    {
        CancelInvoke();
        if (animator)
        {
            bool exists = false;
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name.Contains("Die"))
                {                   
                    animator.SetTrigger("Die");                   
                    gameObject.layer = 13;
                    exists = true;
                }    
            }
            if (!exists)
            {
                Destroy(gameObject);
            }
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void dropLoot()
    {
        if (dropTable && !droppedLoot)
        {
            Pickup item = dropTable.dropItem();
            if (item)
            {
                Instantiate(item, transform.position, Quaternion.identity);
            }
            droppedLoot = true;
        }
    }

    //käytetään kuolemis animaatiossa
    void Destroy()
    {      
        Destroy(gameObject);
    }

    void SetStartHealth()
    {
        if(!gameObject.name.Contains("Slime"))
            health = startHealth;
    }
}
