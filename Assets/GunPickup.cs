using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GunData gunData; // Assign the GunData for this pickup in the Inspector
    private CharacterScript playerScript;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
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
            Destroy(gameObject);
        }
    }
}