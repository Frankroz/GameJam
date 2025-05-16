using UnityEngine;

public class ReloadingAnim : MonoBehaviour
{
    private Shooting shootingScript;
    private SpriteRenderer[] arms;
    private Transform leftArm;
    private Transform rightArm;
    private Quaternion leftArmStartRotation;
    private Quaternion rightArmStartRotation;
    private CharacterScript characterScript;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        arms = GetComponentsInChildren<SpriteRenderer>(true);
        leftArm = arms[0].transform; // Get the Transform
        rightArm = arms[1].transform; // Get the Transform
        leftArmStartRotation = leftArm.localRotation;
        rightArmStartRotation = rightArm.localRotation;
        characterScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();

        spriteRenderer = characterScript.spriteRenderer;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingScript.isReloading)
        {
            float reloadProgress = shootingScript.reloadTimer / shootingScript.reloadingTime;
            float rotationAmount = -45f;

            if (reloadProgress < 0.5f)
            {
                // First half of reload: Rotate outwards
                float currentRotation = rotationAmount * (reloadProgress / 0.5f);
                leftArm.localRotation = leftArmStartRotation * Quaternion.Euler(0, 0, -currentRotation);
                rightArm.localRotation = rightArmStartRotation * Quaternion.Euler(0, 0, currentRotation);
            }
            else
            {
                // Second half of reload: Rotate back to start
                float currentRotation = rotationAmount * ((1f - reloadProgress) / 0.5f);
                leftArm.localRotation = leftArmStartRotation * Quaternion.Euler(0, 0, -currentRotation);
                rightArm.localRotation = rightArmStartRotation * Quaternion.Euler(0, 0, currentRotation);
            }
        }
        else
        {
            leftArm.localRotation = leftArmStartRotation;
            rightArm.localRotation = rightArmStartRotation;
        }

        // Handle invisibility for the arms
        if (characterScript != null && characterScript.isInvisible)
        {
            if (arms.Length >= 2 && arms[0] != null && arms[1] != null)
            {
                Color tempColor = originalColor;
                tempColor.a = 0.7f;
                arms[0].color = tempColor;
                arms[1].color = tempColor;
            }
        }
        else
        {
            // Restore arm visibility
            if (arms.Length >= 2 && arms[0] != null && arms[1] != null)
            {
                arms[0].color = originalColor;
                arms[1].color = originalColor;
            }
        }
    }
}