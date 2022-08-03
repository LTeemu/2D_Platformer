using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ForestArena : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;
    [SerializeField] GameObject boss;
    [SerializeField] Slider bossSlider;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Transform arenaGates;
    private BoxCollider2D arenaCollider;
    [SerializeField] TextMeshProUGUI bossName;

    private void Start()
    {
        arenaCollider = GetComponent<BoxCollider2D>();
        bossName.text = boss.name;
    }

    private void Update()
    {      
        if (boss)
        {
            bossSlider.value = boss.GetComponent<EnemyHealth>().health;            
        } 
        else
        {
            DisableArena();
        }

        if(!player.activeInHierarchy && boss)
        {
            Invoke("RestartScene", 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(boss && animator && audioManager)
            {
                DisablePlayer();
                audioManager.Stop("Theme");
                audioManager.Play("BossCinematic");
                BossCamera();
                Invoke("TakeSword", 1f);
                Invoke("ArenaCamera", 4f);
                Invoke("ActivateBoss", 4f);
                arenaCollider.enabled = false;
            }
        }
    }

    void DisablePlayer()
    {
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.GetComponent<Animator>().enabled = false;
    }

    void EnablePlayer()
    {
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerAttack>().enabled = true;
        player.GetComponent<Animator>().enabled = true;
    }

    private void OnDisable()
    {
        if(animator)
        {
            animator.Play("PlayerCam");
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene("ForestBoss");
    }

    void BossCamera()
    {
        animator.Play("BossCam");
    }

    void ActivateBoss()
    {
        arenaGates.position = new Vector3(arenaGates.position.x, arenaGates.position.y - 3, 0);
        audioManager.Stop("BossCinematic");
        audioManager.Play("Boss");
        boss.GetComponent<ForestBossController>().enabled = true;       
        bossSlider.maxValue = boss.GetComponent<EnemyHealth>().health;
        bossSlider.gameObject.SetActive(true);
        EnablePlayer();
    }

    void DisableArena()
    {       
        bossSlider.gameObject.SetActive(false);
        audioManager.Stop("Boss");
        audioManager.Play("Theme");
        gameObject.SetActive(false);
    }

    void ArenaCamera()
    {
        animator.Play("ArenaCam");
    }

    void TakeSword()
    {
        boss.GetComponent<Animator>().SetTrigger("TakeSword");
        Invoke("SwordSound", 1f);
    }

    void SwordSound()
    {
        audioManager.Play("SwordSwing");
    }
}
