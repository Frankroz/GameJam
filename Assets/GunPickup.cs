using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GunData gunData; // Assign the GunData for this pickup in the Inspector
    private CharacterScript playerScript;
    private PlayerAudioManager audioManager;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<PlayerAudioManager>();
        if (gunData == null)
        {
            Debug.LogError("Gun Data not assigned to the Gun Pickup!");
            Destroy(gameObject); // Destroy the pickup if no data is assigned
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerScript != null && gunData != null)
        {
            playerScript.ChangeGun(gunData);
            if (gunData.gunName == "Shotgun")
                audioManager.PlaySFX(audioManager.shotgunReload);
            if (gunData.gunName == "Rifle")
                audioManager.PlaySFX(audioManager.rifleReload);
            else
                audioManager.PlaySFX(audioManager.pistolReload);
            Destroy(gameObject);
        }
    }
}