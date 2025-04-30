using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerGun : MonoBehaviourPun
{
    public Gun currentGun; // Reference to the Gun scriptable object
    Rigidbody2D rb;
    bool isShooting = false; // Flag to check if the player is shooting

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

        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            StartCoroutine(FireCoroutine());
        }

        // Stop shooting when mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
    }



    private IEnumerator FireCoroutine()
    {
        isShooting = true;

        while (isShooting)
        {
            Shoot();

            // Wait based on the gun's fire rate
            yield return new WaitForSeconds(1f / currentGun.fireRate);
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
