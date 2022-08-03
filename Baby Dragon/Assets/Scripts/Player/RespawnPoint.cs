using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] float respawnCooldown;
    private float respawnCooldownTimer = 0;
    private GameObject player;
    public Transform spawnParticles;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        Respawn();
    }

    private void Respawn()
    {
        if (!player.activeInHierarchy)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.GetComponent<PlayerHealth>().ResetPlayer();

            if (respawnCooldownTimer > respawnCooldown)
            {
                player.transform.position = transform.position;
                player.SetActive(true);
                respawnCooldownTimer = 0;
                Instantiate(spawnParticles, transform.position, transform.rotation);
            }
            else
            {
                respawnCooldownTimer += Time.deltaTime;
            }
        }
    }
}

