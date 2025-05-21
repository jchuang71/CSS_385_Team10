using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Gun")]
public class Gun : ScriptableObject
{
    [SerializeField] public string gunName; // Name of the gun
    [SerializeField] public string gunDescription; // Description of the gun
    [SerializeField] public int gunSelectorIndex = -1; // Description of the gun
    [SerializeField] public float fireRate; // Rate of fire in shots per second
    [SerializeField] public float damage; // Damage dealt by the gun
    [SerializeField] public float range; // Range of the gun in units
    [SerializeField] public float percentangeSpeedDebuff = 0; // Percentage speed debuff applied to the player
    [SerializeField] public float bulletSpeed; // Speed of the bullet in units per second
    [SerializeField] public float bulletSplashDamageRange = 0.5f; // Splash damage range of bullet on hit or impact
    [SerializeField] public float bulletsPerShot = 1; // Number of bullets fired per shot
    [SerializeField] public float bulletSpread = 0; // Spread of the bullets in degrees
    [SerializeField] public float reloadTime; // Time taken to reload the gun in seconds
    [SerializeField] public int magazineSize; // Number of bullets in the magazine
    [SerializeField] public Sprite gunSprite; // Sprite representing the gun
    [SerializeField] public string bulletPrefabPath; // Prefab of the bullet to be instantiated when firing    
    [SerializeField] public AudioClip reloadSound; // Sound played when reloading
}
