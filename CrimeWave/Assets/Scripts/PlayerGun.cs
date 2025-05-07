using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGun : MonoBehaviourPun
{
    public List<Gun> guns; // List of available guns
    public Gun currentGun; // Reference to the Gun scriptable object
    Rigidbody2D rb;
    bool isHoldingFire = false; // Flag to check if the player is shooting
    float lastFireTime = 0f; // Time of the last shot fired
    public AudioSource gunSounds; // Reference to the gun's audio source
    public AudioClip gunshot;
    private UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // loads in all guns from the Resources folder
        guns = new List<Gun>(Resources.LoadAll<Gun>("ScriptableObjects/Guns"));
        // sorts the guns by their selector index
        guns.Sort((a, b) => a.gunSelectorIndex.CompareTo(b.gunSelectorIndex));

        currentGun = guns[0]; // Set the first gun as the initial gun

        uiManager = UIManager.UIManagerInstance; // Get the UIManager instance
        if(photonView.IsMine)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        // Detect mouse held down
        if (Input.GetMouseButtonDown(0))
        {
            isHoldingFire = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHoldingFire = false;
        }

        // Handle firing
        if (isHoldingFire && Time.time >= lastFireTime + (1f / currentGun.fireRate))
        {
            Shoot();
            lastFireTime = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            int currentGunIndex = guns.IndexOf(currentGun);
            int nextGunIndex = (currentGunIndex + 1) % guns.Count;
            SwitchGun(guns[nextGunIndex]);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            int currentGunIndex = guns.IndexOf(currentGun);
            int nextGunIndex = (currentGunIndex - 1) % guns.Count;
            SwitchGun(guns[nextGunIndex]);
        }
    }
    public void Shoot()
    {
        // Implement shooting logic here, e.g., instantiate a bullet prefab, play sound, etc.
        // Play the gun sound
        gunSounds.clip = gunshot;
        gunSounds.Play();
        Debug.Log("Shooting with " + currentGun.gunName);
        for (int i = 0; i < currentGun.bulletsPerShot; i++)
        {
            FireBullet();
        }
    }

    public void FireBullet()
    {
        //creates a random spread for the bullet
        float spread = Random.Range(-currentGun.bulletSpread, currentGun.bulletSpread);

        GameObject bullet = PhotonNetwork.Instantiate(
            currentGun.bulletPrefabPath, 
            transform.position, 
            Quaternion.Euler(0, 0, rb.rotation),
            0,
            new object[] { spread } // Send spread angle as instantiation data
        );

        bullet.GetComponent<BulletLogic>().SetBulletData(currentGun); // Set the bullet data from the gun
        bullet.GetComponent<BulletLogic>().SetShooterViewID(photonView.ViewID); // Set the shooter view ID
        bullet.transform.up = transform.up; // Set the bullet's direction to the player's direction
    }

    void SwitchGun(Gun newGun)
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found! Make sure it is in the scene."); // Error message if UIManager is not found
            return;
        }
        Debug.Log(newGun.gunName + " is now selected.");
        currentGun = newGun; // Switch to the new gun
        uiManager.SetWeaponSelectorImage(currentGun.gunSprite); // Update the UI with the new gun icon
        Debug.Log("Switched to " + currentGun.gunName);
    }
}
