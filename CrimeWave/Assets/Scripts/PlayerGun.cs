using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

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
    private Dictionary<Gun, int> currentAmmo = new Dictionary<Gun, int>(); // Tracks current ammo for each gun
    private bool isReloading = false; // Flag to check if player is reloading
    private float reloadEndTime = 0f; // Time when current reload will finish
    public AudioClip reloadSoundEffect; // General reload sound if gun doesn't have one


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

        foreach (Gun gun in guns)
        {
            currentAmmo[gun] = gun.magazineSize;
        }

        // Update UI with initial ammo count
        UpdateAmmoUI();

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        // Check if reload is complete
        if (isReloading && Time.time >= reloadEndTime)
        {
            // Reload complete
            currentAmmo[currentGun] = currentGun.magazineSize;
            isReloading = false;

            // Hide reloading text using UIManager
            if (uiManager != null)
            {
                uiManager.SetReloadingTextVisible(false);
            }

            // Hide reloading indicator for all players
            //photonView.RPC("PlayReloadAnimation", RpcTarget.All, false);

            // Update UI
            UpdateAmmoUI();
        }


        // Handle manual reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo[currentGun] < currentGun.magazineSize)
        {
            StartReload();
        }

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
        if (isHoldingFire && !isReloading && Time.time >= lastFireTime + (1f / currentGun.fireRate))
        {
            Shoot();
            lastFireTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            int currentGunIndex = guns.IndexOf(currentGun);
            int nextGunIndex = (currentGunIndex + 1) % guns.Count;
            SwitchGun(guns[nextGunIndex]);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            int currentGunIndex = guns.IndexOf(currentGun);
            int nextGunIndex = (currentGunIndex - 1 + guns.Count) % guns.Count; // Wrap around safe
            SwitchGun(guns[nextGunIndex]);
        }
    }

    public void StartReload()
    {
        if (!photonView.IsMine || isReloading) return;

        // Don't reload if magazine is already full
        if (currentAmmo[currentGun] >= currentGun.magazineSize) return;

        isReloading = true;
        reloadEndTime = Time.time + currentGun.reloadTime;

        // Play reload sound
        AudioClip soundToPlay = currentGun.reloadSound != null ? currentGun.reloadSound : reloadSoundEffect;
        if (soundToPlay != null && gunSounds != null)
        {
            gunSounds.clip = soundToPlay;
            gunSounds.Play();
        }

        // Show reloading text using UIManager
        if (uiManager != null)
        {
            uiManager.SetReloadingTextVisible(true);
        }

        // Show reloading indicator for all players in the network
        //photonView.RPC("PlayReloadAnimation", RpcTarget.All, true);

    }

    public void CancelReload()
    {
        if (!photonView.IsMine) return;

        isReloading = false;

        // Hide reloading text using UIManager
        if (uiManager != null)
        {
            uiManager.SetReloadingTextVisible(false);
        }

        // Hide reloading indicator for all players
        //photonView.RPC("PlayReloadAnimation", RpcTarget.All, false);
    }


    private void UpdateAmmoUI()
    {
        if (uiManager != null)
        {
            uiManager.SetAmmoText(currentAmmo[currentGun], currentGun.magazineSize);
        }
    }
    public void Shoot()
    {

        if (!photonView.IsMine) return;

        // Can't shoot if reloading
        if (isReloading) return;

        // Check if we have ammo
        if (currentAmmo[currentGun] <= 0)
        {
            // Play empty magazine sound or just start reloading
            StartReload();
            return;
        }

        // Implement shooting logic here, e.g., instantiate a bullet prefab, play sound, etc.
        // Play the gun sound
        gunSounds.clip = gunshot;
        gunSounds.Play();
        Debug.Log("Shooting with " + currentGun.gunName);
        for (int i = 0; i < currentGun.bulletsPerShot; i++)
        {
            FireBullet();
        }

        // Reduce ammo by 1 for this shot (regardless of bullets per shot)
        currentAmmo[currentGun]--;

        // Update the ammo UI
        UpdateAmmoUI();

        // Auto-reload if magazine is empty
        if (currentAmmo[currentGun] <= 0)
        {
            StartReload();
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

        if (!photonView.IsMine) return;

        // Cancel reload when switching guns
        if (isReloading)
        {
            CancelReload();
        }

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found! Make sure it is in the scene."); // Error message if UIManager is not found
            return;
        }
        Debug.Log(newGun.gunName + " is now selected.");
        currentGun = newGun; // Switch to the new gun
        uiManager.SetWeaponSelectorImage(currentGun.gunSprite); // Update the UI with the new gun icon
        Debug.Log("Switched to " + currentGun.gunName);

        UpdateAmmoUI();

    }
}
