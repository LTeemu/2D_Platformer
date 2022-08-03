using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startHealth;
    public int health;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    [SerializeField] int invulnerabilityTime;
    [HideInInspector] public bool invulnerable = false;
    private Animator animator;

    private void Start()
    {
        health = startHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(health <= 0)
            gameObject.SetActive(false);

        UiHearts();
    }

    public void TakeDamage(int damage)
    {
        if (!invulnerable) 
        {                     
            SetHealth(health - damage);
            Debug.Log("Player took " + damage + " damage. Remaining: " + health);
            StartCoroutine("invulnerabilityFrames");          
        }
    }

    public void ResetPlayer()
    {
        health = startHealth;
        gameObject.GetComponent<PlayerAttack>().addFireballs(5);
        setVulnerable();
    }

    void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void PlusOne()
    {
        if(health < 5)
        {
            health++;
        }
    }

    void UiHearts()
    {
        for (int i = 0; i < hearts.Length; i++) {
            if (i < health) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < startHealth) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }     
    }

    void setInvulnerable()
    {
        invulnerable = true;
        animator.SetBool("Invulnerable", true);
    }

    void setVulnerable()
    {
        invulnerable = false;
        animator.SetBool("Invulnerable", false);
    }

    private IEnumerator invulnerabilityFrames()
    {
        setInvulnerable();
        yield return new WaitForSeconds(invulnerabilityTime);
        setVulnerable();
    }
}
