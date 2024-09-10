using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;

    private float immunity_time = 0f;
    public int maxHealth = 3;
    public int currentHealth = 3;

    Vector2 movement;
    Vector2 mousePos;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.deltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    public void TakeDamage()
    {
        if (Time.time > immunity_time)
        {

            currentHealth--;
            if (currentHealth <= 0)
            {
                Debug.Log("die");
            }
            immunity_time = Time.time + 1f;
        }
    }
}
