using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Items/Gun")]
public class GunData : ScriptableObject
{
    public string gunName = "Default Gun";
    public float fireRate = 0.1f; // Time between shots
    public int ammoCapacity = 12;
    public GameObject bulletPrefab; // The bullet prefab this gun shoots
}