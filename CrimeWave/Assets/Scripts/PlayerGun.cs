using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerGun : MonoBehaviourPun
{
    public Gun currentGun; // Reference to the Gun scriptable object
    Rigidbody2D rb;
    bool isHoldingFire = false; // Flag to check if the player is shooting
    float lastFireTime = 0f; // Time of the last shot fired

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }
    public void Shoot()
    {
        // Implement shooting logic here, e.g., instantiate a bullet prefab, play sound, etc.
        Debug.Log("Shooting with " + currentGun.gunName);
        GameObject bullet = PhotonNetwork.Instantiate(
            currentGun.bulletPrefabPath, 
            transform.position, 
            Quaternion.Euler(0, 0, rb.rotation)
        );

        bullet.GetComponent<BulletLogic>().SetBulletData(currentGun); // Set the bullet data from the gun
        bullet.transform.up = transform.up; // Set the bullet's direction to the player's direction
    }
}
