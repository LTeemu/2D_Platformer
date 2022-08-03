using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetimeMax;
    private float lifetime;
    private float direction;
    [SerializeField] int damage;

    [Header("References")]
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, 0, 0);

        if (lifetime < lifetimeMax)
        {
            lifetime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Ranger hit " + collision.gameObject.name);
        Destroy(gameObject);
        if (collision.CompareTag("Player")) {
            //collision.gameObject.SetActive(false);
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
