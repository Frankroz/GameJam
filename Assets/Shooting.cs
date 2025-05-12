using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera mainCamera;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    private bool canFire = true;
    private float timer;
    public float timeBetweenFiring;
    public float reloadingTime = 3f;
    private float bulletSpawnOffset = 0.5f;
    public static int maxBullets = 12;
    private int bulletCount = maxBullets;
    public Text bulletCountText;
    private bool isReloading = false;
    private float reloadTimer;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;

        float lookAt = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 135f;

        transform.rotation = Quaternion.Euler(0, 0, lookAt);

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadingTime)
            {
                bulletCount = maxBullets;
                isReloading = false;
                UpdateBulletText();
                canFire = true; // Allow firing after reload
            }
            return; // Don't process firing input while reloading
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }



        if (Input.GetMouseButton(0) && canFire && bulletCount > 0)
        {
            canFire = false;
            bulletCount--;
            UpdateBulletText();
            Vector3 spawnPosition = bulletTransform.position + bulletTransform.up * bulletSpawnOffset;
            Instantiate(bullet, spawnPosition, Quaternion.Euler(0, 0, transform.eulerAngles.z - 45f));

            if (bulletCount <= 0)
            {
                StartReload();
            }
        }
        else if (Input.GetMouseButton(0) && canFire && bulletCount <= 0 && !isReloading)
        {
            StartReload(); // Auto-reload if trying to fire with no bullets
        }
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = 0f;
        canFire = false;
        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        if (bulletCountText != null)
        {
            if (isReloading)
            {
                bulletCountText.fontSize = 80;
                bulletCountText.text = "Reloading...";
            }
            else
            {
                bulletCountText.fontSize = 160;
                bulletCountText.text = $"{bulletCount}/{maxBullets}";
            }
        }
        else
        {
            Debug.LogWarning("Bullet Count Text UI element not assigned!");
        }
    }
}
