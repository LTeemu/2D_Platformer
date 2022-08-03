using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] spawnList;
    [SerializeField] float spawnCooldown;
    private float spawnCooldownTimer;
    public List<GameObject> spawnedEnemies;
    [SerializeField] int spawnLimit;
    private Animator animator;
    private GameObject player;
    [SerializeField] Animator gateAnimator;
    [SerializeField] Transform spawnPosition;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (player.activeInHierarchy && distToPlayer < 50)
        {
            SpawnRandom();
        }
    }

    private void SpawnRandom()
    {
        
        if (spawnCooldownTimer >= spawnCooldown)
        {                           
            for(int i = 0; i < spawnLimit; i++)
            {
                if(spawnedEnemies.Count == spawnLimit) 
                {
                    if (spawnedEnemies[i] == null)
                    {
                        spawnedEnemies.RemoveAt(i);
                    }
                }
                if(spawnedEnemies.Count < spawnLimit)
                {
                    gateAnimator.SetBool("isOpen", true);
                }
                if(spawnCooldownTimer < spawnCooldown)
                {
                    break;
                }
            }          
        }
        else
        {
            spawnCooldownTimer += Time.deltaTime;
        }
    }

    void spawnEnemy()
    {
        int random = Random.Range(0, spawnList.Length);
        GameObject spawnedEnemy = Instantiate(spawnList[random], spawnPosition.position, Quaternion.identity);
        spawnedEnemies.Add(spawnedEnemy);
        spawnCooldownTimer = 0;
        gateAnimator.SetBool("isOpen", false);
    }
}

