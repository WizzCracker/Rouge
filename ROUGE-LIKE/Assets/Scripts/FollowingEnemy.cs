using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : MonoBehaviour
{
    [SerializeField] float health = 5f;
    public GameObject deathEffect;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        GetComponent<Collider2D>().enabled = false;
        if(gameObject.transform.parent.transform.childCount == 1)
        {
            
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (collision.tag == "Player")
        {
            player.TakeDamage();
        }
    }

}
