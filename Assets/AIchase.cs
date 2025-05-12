using UnityEngine;
using UnityEngine.UI; // Important for UI elements

public class AIchase : MonoBehaviour
{
    private GameObject player;
    public float speed;
    public static float maxHealth = 100f; // Store the maximum health
    private float health = maxHealth;
    public float damagePerSecond = 10f;
    private float lastDamageTime;
    private string healthBarName = "HealthBar";

    private Scrollbar healthBar;

    private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    private void Die()
    {
        CharacterScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        playerScript.addScore(1);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= 1f)
            {
                CharacterScript playerScript = collision.gameObject.GetComponent<CharacterScript>();
                if (playerScript != null)
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