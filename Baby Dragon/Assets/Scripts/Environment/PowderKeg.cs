using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKeg : MonoBehaviour
{
    [SerializeField] private GameObject destructibleRef;
    [SerializeField] private GameObject explosionRef;
    [SerializeField] private float shakeAmount;
    private BoxCollider2D coll;
    private Animator anim;
    private bool isShaking = false;
    Vector3 curPos;
    private AudioManager audioManager;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        curPos = transform.position;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (isShaking)
        {
            Vector3 newPos = curPos + Random.insideUnitSphere * (Time.deltaTime * shakeAmount);
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;
            transform.position = newPos;           
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Fireball"))
        {
            anim.enabled = true;
            coll.enabled = false;        }
    }

    private void PowderKegShaker()
    {      
        isShaking = true;    
    }

    private void PowderKegExpl()
    {
        audioManager.Stop("Fuse");
        Instantiate(destructibleRef, coll.bounds.center, Quaternion.identity);
        Instantiate(explosionRef, coll.bounds.center, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void PlayFuse()
    {
        audioManager.Play("Fuse");
    }
}
