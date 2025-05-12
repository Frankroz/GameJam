using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    private float xBoundary = 5.7f;
    private float yBoundary = 7.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float targetX = target.position.x;
        float targetY = target.position.y;

        float clampedX = Mathf.Clamp(targetX, -xBoundary, xBoundary);
        float clampedY = Mathf.Clamp(targetY, -yBoundary, yBoundary);

        Vector3 newPos = new Vector3(clampedX, clampedY, -30f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
