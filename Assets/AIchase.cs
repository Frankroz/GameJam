using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Important for UI elements

public class AIchase : MonoBehaviour
{
    private GameObject player;
    public GameObject blood;
    public float speed;
    public static float maxHealth = 100f; // Store the maximum health
    private float health = maxHealth;
    public float damagePerSecond = 10f;
    private float lastDamageTime;
    private string healthBarName = "HealthBar";
    private CharacterScript playerScript;
    [Range(0f, 1f)] public float dropChance = 0.5f; // Chance (0 to 1) to drop an item
    public GameObject healthPickupPrefab;
    public GameObject[] gunPickupPrefab;
    private Vector3 lastPosition;

    private Scrollbar healthBar;

    private float distance;
    public ZombieAudioManager zombieAudioManager;
    public WardenAudioManager wardenAudioManager;
    public float walkingSoundDuration;
    private float walkingSoundTimer = 0f;
    private bool isWalkingSoundPlaying = false;

    public LogicScript logicScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zombieAudioManager = GetComponentInChildren<ZombieAudioManager>();
        wardenAudioManager = GetComponentInChildren<WardenAudioManager>();
        logicScript = GameObject.FindGameObjectWithTag("LogicScript").GetComponent<LogicScript>();


        if (player != null)
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        lastDamageTime = -damagePerSecond;

        // Find the health bar Scrollbar component in the children
        Transform healthBarTransform = transform.Find(healthBarName);
        if (healthBarTransform != null)
        {
            healthBar = healthBarTransform.GetComponentInChildren<Scrollbar>();
            if (healthBar == null)
            {
                Debug.LogError($"Scrollbar component not found on child GameObject named '{healthBarName}' of {gameObject.name}");
            }
        }
        else
        {
            Debug.LogError($"Child GameObject named '{healthBarName}' not found on {gameObject.name}");
        }

        // Ensure maxHealth is initialized correctly, if not set in the Inspector
        if (maxHealth <= 0)
        {
            maxHealth = health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (logicScript.isGamePaused)
        {
            return;
        }
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }

        if (playerScript != null && playerScript.isInvisible)
        {
            // Player is invisible, stop chasing
            /* Previous collision cannot affect the enemy */
            return;
        }


        if (playerScript != null)
        {
            lastPosition = player.transform.position;
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;

            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);

            bool isMoving = direction.magnitude > 0.1f;

            if (isMoving)
            {
                direction = direction.normalized;
                if (!isWalkingSoundPlaying)
                {

                    if (gameObject.name == "Zombie(Clone)")
                        { zombieAudioManager.PlayWalking(); }
                    else
                        { wardenAudioManager.PlayWalking(); }
                    
                    isWalkingSoundPlaying = true;
                    walkingSoundTimer = 0f;
                }
                else
                {
                    walkingSoundTimer += Time.deltaTime;
                    if (walkingSoundTimer >= walkingSoundDuration)
                    {
                        walkingSoundTimer = 0f;
                        if (gameObject.name == "Zombie(Clone)")
                            zombieAudioManager.PlayWalking();
                        else
                            wardenAudioManager.PlayWalking();
                    }
                }
            }
        } else
        {
            distance = Vector2.Distance(transform.position, lastPosition);
            Vector2 direction = lastPosition - transform.position;
            direction.Normalize();

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;

            transform.position = Vector2.MoveTowards(this.transform.position, lastPosition, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    private void Die()
    {
        if (playerScript != null)
        {
            playerScript.addScore(1);
        }

        if (Random.value <= dropChance)
        {
            // Decide what to drop
            if (Random.value < 0.5f && healthPickupPrefab != null)
            {
                Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
            }
            else if (gunPickupPrefab != null)
            {
                int rand = Random.Range(0, gunPickupPrefab.Length);
                Instantiate(gunPickupPrefab[rand], transform.position, Quaternion.identity);
            }
        }

        if (gameObject.name == "Zombie(Clone)")
            { zombieAudioManager.PlayDamage(); }
        else
            { wardenAudioManager.PlayDamage(); }
        Destroy(gameObject);
        Instantiate(blood, new Vector3(transform.position.x, transform.position.y, 5), Quaternion.identity);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= 1f)
            {
                CharacterScript playerScript = collision.gameObject.GetComponent<CharacterScript>();
                if (playerScript != null && !playerScript.isInvisible)
                {
                    playerScript.TakeDamage(damagePerSecond);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = health / maxHealth;
            healthBar.size = healthPercentage;
        }
    }

}