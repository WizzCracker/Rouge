using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1;
    public Rigidbody2D rb;
    public GameObject hitEffect;

    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag != "Player")
        {
            FollowingEnemy enemy = hitInfo.GetComponent<FollowingEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.1f);
            Destroy(gameObject);
        }
    }
}
