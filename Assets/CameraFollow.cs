using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    private float xMin = -16f; // Left wall X position
    private float xMax = 16f;  // Right wall X position
    private float yMin = -13f; // Bottom wall Y position
    private float yMax = 13f;  // Top wall Y position

    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("CameraFollow script needs to be attached to a Camera GameObject.");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || mainCamera == null) return;

        float targetX = target.position.x;
        float targetY = target.position.y;

        // Calculate the orthographic size and aspect ratio to determine visible boundaries
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        // Clamp the target position within the world boundaries, considering the camera's viewport
        float clampedX = Mathf.Clamp(targetX, xMin + halfWidth, xMax - halfWidth);
        float clampedY = Mathf.Clamp(targetY, yMin + halfHeight, yMax - halfHeight);

        Vector3 newPos = new Vector3(clampedX, clampedY, -30f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}