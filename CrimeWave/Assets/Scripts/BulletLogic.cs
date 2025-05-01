using UnityEngine;
using Photon.Pun;

public class BulletLogic : MonoBehaviourPun
{
    public float speed; // Speed of the bullet
    public float range; // Range of the bullet
    public float damage; // Damage dealt by the bullet
    private float distanceTravelled; // Distance traveled by the bullet

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (photonView.IsMine)
        {
            distanceTravelled = 0f; // Initialize distance travelled to 0
        }
    }

    // Update is called once per frame
    void Update()
    {
        BulletMovement(); // Call the BulletMovement function every frame
    }

    public void SetBulletData(Gun gun)
    {
        if (photonView.IsMine)
        {
            speed = gun.bulletSpeed; // Set the bullet speed from the gun data
            range = gun.range; // Set the bullet range from the gun data
            damage = gun.damage; // Set the bullet damage from the gun data
        }
    }

    void BulletMovement()
    {
        if (photonView.IsMine)
        {
            distanceTravelled += speed * Time.deltaTime;

            // Move the bullet forward in the direction it is facing
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            
            // Check if the bullet has exceeded its range
            if (distanceTravelled > range)
            {
                PhotonNetwork.Destroy(gameObject); // Destroy the bullet if it exceeds its range
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag("Destructible"))
            {
                PhotonNetwork.Destroy(other.gameObject); // Destroy target
                PhotonNetwork.Destroy(gameObject); // Destroy bullet
            }
        }
    }
}
