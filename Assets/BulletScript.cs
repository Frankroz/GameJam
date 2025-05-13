using System;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float force;
    private Vector2 initialDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        
        if (initialDirection.y != 0f && initialDirection.x != 0f)
        {
            rb.linearVelocity = initialDirection * force;
        } else
        {
            rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force * 3;
        }
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rot + 90, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirection(Vector3 direction)
    {
        initialDirection = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            AIchase enemyAI = collision.gameObject.GetComponent<AIchase>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(25);
            }
            Destroy(gameObject);
        }
    }
}
