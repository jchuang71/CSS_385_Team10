using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Gun")]
public class Gun : ScriptableObject
{
    [SerializeField] public string gunName; // Name of the gun
    [SerializeField] public string gunDescription; // Description of the gun
    [SerializeField] public int gunSelectorIndex; // Description of the gun
    [SerializeField] public float fireRate; // Rate of fire in shots per second
    [SerializeField] public float damage; // Damage dealt by the gun
    [SerializeField] public float range; // Range of the gun in units
    [SerializeField] public float bulletSpeed; // Speed of the bullet in units per second
    [SerializeField] public float bulletSplashDamageRange; // Splash damage range of bullet on hit or impact
    [SerializeField] public float bulletsPerShot; // Number of bullets fired per shot
    [SerializeField] public float bulletSpread; // Spread of the bullets in degrees
    [SerializeField] public float reloadTime; // Time taken to reload the gun in seconds
    [SerializeField] public int magazineSize; // Number of bullets in the magazine
    [SerializeField] public Sprite gunSprite; // Sprite representing the gun
    [SerializeField] public string bulletPrefabPath; // Prefab of the bullet to be instantiated when firing    
}
