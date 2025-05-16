using UnityEngine;

public class ChangeGunSprite : MonoBehaviour
{
    private SpriteRenderer[] gunRenderers;
    private Shooting shootingScript;
    private GunData currentGun;

    void Start()
    {
        shootingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        gunRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        
        UpdateGunVisibility();
    }

    void Update()
    {
        UpdateGunVisibility();
    }

    void UpdateGunVisibility()
    {
        currentGun = shootingScript.currentGun;

        if (currentGun == null)
        {
            // Disable all gun sprites if no current gun is assigned
            foreach (SpriteRenderer renderer in gunRenderers)
            {
                renderer.enabled = false;
            }
            return;
        }

        bool foundMatchingGun = false;

        foreach (SpriteRenderer renderer in gunRenderers)
        {

            // Assuming the name of the child GameObject matches the gunName in GunData
            if (renderer.gameObject.name == currentGun.gunName)
            {
                renderer.enabled = true;
                foundMatchingGun = true;
            }
            else
            {
                renderer.enabled = false;
            }
        }

        if (!foundMatchingGun)
        {
            Debug.LogWarning($"No child GameObject with the name '{currentGun.gunName}' found to display the current gun.");
        }
    }
}