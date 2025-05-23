using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;
    public GunData currentGun;
    public GunData defaultGun;
    public GameObject bulletCase;
    public GameObject[] gunFires;
    public GameObject bulletPrefab;
    public Transform bulletTransform;
    private bool canFire = true;
    private float timer;
    public float timeBetweenFiring; // Set by currentGun
    public float reloadingTime = 3f;
    private float bulletSpawnOffset = 0.5f;
    public int maxBullets; // Set by currentGun
    public int bulletCount;
    public Text bulletCountText;
    public bool isReloading = false;
    public float reloadTimer;
    private int shotgunPelletCount = 5;
    private float shotgunSpreadAngle = 45f;
    private float gunfireDuration = 0.1f;

    public PlayerAudioManager audioManager;
    public LogicScript logicScript;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (currentGun != null)
        {
            timeBetweenFiring = currentGun.fireRate;
            maxBullets = currentGun.ammoCapacity;
            bulletCount = currentGun.ammoCapacity;
            UpdateBulletText();
        }
        else if (defaultGun != null)
        {
            EquipGun(defaultGun); // Equip the default gun at start if no other gun is set
        }
        else
        {
            Debug.LogWarning("No initial or default gun assigned to the Shooting script!");
        }
    }

    void Update()
    {
        if(logicScript.isGamePaused) { return; }

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
                canFire = true;
            }
            return;
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

        if (Input.GetMouseButton(0) && canFire && bulletCount > 0 && currentGun != null && bulletPrefab != null)
        {
            canFire = false;
            bulletCount--;
            UpdateBulletText();
            Vector3 spawnPosition = bulletTransform.position + bulletTransform.up * bulletSpawnOffset;
            Quaternion baseRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);

            if (currentGun.gunName == "Shotgun")
            {
                FireShotgun(spawnPosition, baseRotation, direction);
            }
            else
            {
                if (currentGun.gunName == "Rifle")
                    audioManager.PlaySFX(audioManager.rifleGunshot);
                else
                    audioManager.PlaySFX(audioManager.pistolGunshot);
                Instantiate(bulletPrefab, spawnPosition, baseRotation);
            }

            if (bulletCount <= 0)
            {
                StartReload();
            }

            Instantiate(bulletCase, spawnPosition, baseRotation);
            int rand = Random.Range(0, gunFires.Length);
            GameObject gunfireInstance = Instantiate(gunFires[rand], spawnPosition, Quaternion.Euler(0, 0, transform.eulerAngles.z + 45f));
            StartCoroutine(DisableAfterDelay(gunfireInstance, gunfireDuration));
        }
        else if (Input.GetMouseButton(0) && canFire && bulletCount <= 0 && !isReloading)
        {
            StartReload();
        }
    }

    void FireShotgun(Vector3 spawnPosition, Quaternion baseRotation, Vector3 baseDirection)
    {
        float halfSpread = shotgunSpreadAngle / 2f;
        for (int i = 0; i < shotgunPelletCount; i++)
        {
            audioManager.PlaySFX(audioManager.shotgunGunshot);
            float spread = Random.Range(-halfSpread, halfSpread);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
            Quaternion finalRotation = baseRotation * spreadRotation;
            Vector3 spreadDirection = spreadRotation * baseDirection;
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, finalRotation);

            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(spreadDirection);
            }
        }
    }

    IEnumerator DisableAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = 0f;
        canFire = false;
        UpdateBulletText();

        if (currentGun.gunName == "Shotgun")
            audioManager.PlaySFX(audioManager.shotgunReload);
        if (currentGun.gunName == "Rifle")
            audioManager.PlaySFX(audioManager.rifleReload);
        else
            audioManager.PlaySFX(audioManager.pistolReload);

        // Check if the current gun is different from the default gun
        if (currentGun != defaultGun && isReloading == true)
        {
            // Start a coroutine to switch to the default gun after reloading
            StartCoroutine(SwitchToDefaultGunAfterReload());
        }
    }

    IEnumerator SwitchToDefaultGunAfterReload()
    {
        // Wait for the reload to finish
        yield return new WaitUntil(() => !isReloading);

        // Equip the default gun
        EquipGun(defaultGun);
    }

    public void EquipGun(GunData newGunData)
    {
        // Interrupt reloading if a new gun is picked up
        isReloading = false;
        reloadTimer = 0f;
        canFire = true; // Player can fire immediately with the new gun
        currentGun = newGunData;
        timeBetweenFiring = currentGun.fireRate;
        maxBullets = currentGun.ammoCapacity;
        bulletCount = currentGun.ammoCapacity;
        UpdateBulletText();
    }

    public void UpdateBulletText()
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