using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public GameObject blood;
    public Text scoreText;
    public Scrollbar manaBar; // Assign the mana Scrollbar in the Inspector
    public float velocity;
    public float maxHealth = 100f;
    public float health = 100f;
    public int score = 0;
    public float maxMana = 100f;
    public float currentMana = 100f;
    public float manaCostPerSecond = 20f;
    public float manaRegenPerSecond = 10f;
    public float invisibilityDuration = 5f; // Total duration of invisibility
    private float invisibilityTimer = 0f;
    public bool isInvisible = false;
    public float invisibilityCooldown = 3f; // Cooldown after invisibility ends
    private float invisibilityCooldownTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public Shooting shootingScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        UpdateManaBar();
        UpdateScoreText();
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

        HandleInvisibilityInput();
        HandleInvisibility();
        RegenerateMana();
        UpdateManaBar();
    }

    void HandleInvisibilityInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInvisible && currentMana >= manaCostPerSecond * Time.deltaTime && invisibilityCooldownTimer <= 0)
        {
            isInvisible = true;
            invisibilityTimer = invisibilityDuration;
        } else if (Input.GetKeyDown(KeyCode.E) && isInvisible)
        {
            isInvisible = false;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
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

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogWarning("Score Text UI element not assigned!");
        }
    }

    void HandleInvisibility()
    {
        if (isInvisible)
        {
            invisibilityTimer -= Time.deltaTime;
            currentMana -= manaCostPerSecond * Time.deltaTime;

            // Make the player visually transparent
            if (spriteRenderer != null)
            {
                Color tempColor = originalColor;
                tempColor.a = 0.7f; // Adjust alpha for transparency
                spriteRenderer.color = tempColor;
            }

            if (currentMana <= 0 || invisibilityTimer <= 0)
            {
                isInvisible = false;
                invisibilityCooldownTimer = invisibilityCooldown;
                // Restore original color
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = originalColor;
                }
            }
        }
        else if (invisibilityCooldownTimer > 0)
        {
            invisibilityCooldownTimer -= Time.deltaTime;
        }
        else
        {
            if (spriteRenderer != null && spriteRenderer.color != originalColor)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    void RegenerateMana()
    {
        if (!isInvisible && currentMana < maxMana)
        {
            currentMana += manaRegenPerSecond * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        }
    }

    void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.size = currentMana / maxMana;
        }
        else
        {
            Debug.LogWarning("Mana Scrollbar not assigned in the Inspector!");
        }
    }

    public void addHealth(float amount)
    {
        if (health < maxHealth)
            health += amount;
    }

    public void ChangeGun(GunData newGunData)
    {
        if (shootingScript != null && newGunData != null)
        {
            shootingScript.currentGun = newGunData;
            shootingScript.timeBetweenFiring = newGunData.fireRate;
            shootingScript.maxBullets = newGunData.ammoCapacity;
            shootingScript.bulletCount = newGunData.ammoCapacity;
            shootingScript.UpdateBulletText();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement player death logic here (e.g., game over screen)
        Instantiate(blood, new Vector3(transform.position.x, transform.position.y, 5), Quaternion.identity);
        Destroy(gameObject);
    }
}
