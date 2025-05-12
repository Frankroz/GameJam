using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Transform parent;

    void Start()
    {
        // Find the main camera in the scene
        Camera mainCamera = Camera.main;
        parent = transform.parent;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found in the scene.");
            enabled = false; // Disable the script if no camera is found
        }
    }

    void LateUpdate()
    {
        // Ensure the camera reference is still valid
        if (mainCameraTransform != null)
        {
            // Make the UI element look at the camera
            transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                             mainCameraTransform.rotation * Vector3.up);

            transform.position = parent.transform.position + new Vector3(0f, 0.7f, 0f);
        }
    }
}