using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public Text scoreText;
    public float velocity;
    public float health = 100f;
    public int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector2.right;
        }

        if (moveDirection.magnitude > 1f)
        {
            moveDirection = moveDirection.normalized;
        }

        myRigidbody.MovePosition(myRigidbody.position + (moveDirection * velocity));
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Player took {amount} damage. Current health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    public void addScore(int plusScore)
    {
        score += plusScore;
        scoreText.text = score.ToString();
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement player death logic here (e.g., game over screen)
        Destroy(gameObject);
    }
}
