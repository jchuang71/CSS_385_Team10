using Photon.Pun;
using UnityEngine;

public class PlayerGun : MonoBehaviourPun
{
    public Gun currentGun; // Reference to the Gun scriptable object
    Rigidbody2D rb;

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
        Shoot(); // Call the Shoot function every frame
    }

    public void Shoot()
    {
        if (photonView.IsMine)
        {
            if(Input.GetMouseButtonDown(0)) // Check if the left mouse button is pressed
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
    }
}
