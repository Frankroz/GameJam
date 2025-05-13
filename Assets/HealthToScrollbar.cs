using UnityEngine;
using UnityEngine.UI;

public class HealthToScrollbar : MonoBehaviour
{
    public CharacterScript playerCharacter; // Reference to your player's CharacterScript
    private Scrollbar healthScrollbar;
    private float maxHealth; // Store the player's maximum health

    void Start()
    {
        // Get the Scrollbar component attached to this GameObject
        healthScrollbar = GetComponent<Scrollbar>();

        // Ensure the playerCharacter reference is set
        if (playerCharacter == null)
        {
            Debug.LogError("Player CharacterScript not assigned to the HealthToScrollbar script on " + gameObject.name);
            enabled = false; // Disable the script if the reference is missing
            return;
        }

        // Get the player's maximum health from the CharacterScript
        maxHealth = playerCharacter.maxHealth;

        // Initialize the scrollbar size based on the initial health
        UpdateScrollbarSize();
    }

    void Update()
    {
        // Update the scrollbar size every frame to reflect changes in health
        UpdateScrollbarSize();
    }

    void UpdateScrollbarSize()
    {
        if (healthScrollbar != null && playerCharacter != null && maxHealth > 0)
        {
            // Calculate the proportional health (0 to 1)
            float healthRatio = playerCharacter.health / maxHealth;

            // Set the scrollbar's size to this ratio
            healthScrollbar.size = Mathf.Clamp01(healthRatio);
        }
    }
}