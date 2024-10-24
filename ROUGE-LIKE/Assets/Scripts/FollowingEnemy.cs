using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : MonoBehaviour
{
    [SerializeField] float health = 5f;
    public GameObject deathEffect;
    private Rigidbody2D rb;
    //private Transform currentPoint;
    private Transform target;
    public float speed;
    Vector2 lookDir;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        if (target != null)
        {
            lookDir = (Vector2)target.position - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            transform.position += transform.up * speed * Time.deltaTime;
        }
        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
        if(gameObject.transform.parent.transform.childCount == 5)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (collision.tag == "Player")
        {
            player.TakeDamage();
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();
            player.TakeDamage();
        }
    }

}
