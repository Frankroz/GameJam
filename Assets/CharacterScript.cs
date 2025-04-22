using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float velocity;

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

        myRigidbody.MovePosition(myRigidbody.position + (moveDirection * velocity * Time.deltaTime));
    }
}
