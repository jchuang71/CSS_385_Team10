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
        guns = new List<Gun>(guns); // Initialize the list of guns
        
        currentGun = guns[0]; // Set the first gun as the current gun
        uiManager = FindObjectOfType<UIManager>();
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
            // Switch to the next gun in the array
            int currentGunIndex = System.Array.IndexOf(currentGun, currentGun);
            int nextGunIndex = (currentGunIndex + 1) % currentGun.Length; // Loop back to the first gun if at the end
            SwitchGun(currentGun[nextGunIndex]); // Switch to the next gun
        }
    }
    public void Shoot()
    {
        // Implement shooting logic here, e.g., instantiate a bullet prefab, play sound, etc.
        // Play the gun sound
        gunSounds.clip = gunshot;
        gunSounds.Play();
        Debug.Log("Shooting with " + currentGun.gunName);
        GameObject bullet = PhotonNetwork.Instantiate(
            currentGun.bulletPrefabPath, 
            transform.position, 
            Quaternion.Euler(0, 0, rb.rotation)
        );

        bullet.GetComponent<BulletLogic>().SetBulletData(currentGun); // Set the bullet data from the gun
        bullet.GetComponent<BulletLogic>().SetShooterViewID(photonView.ViewID); // Set the shooter view ID
        bullet.transform.up = transform.up; // Set the bullet's direction to the player's direction
    }

    void SwitchGun(Gun newGun)
    {
        currentGun = newGun; // Switch to the new gun
        
        Debug.Log("Switched to " + currentGun.gunName);
    }
}
