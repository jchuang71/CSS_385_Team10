using UnityEngine;
using Photon.Pun;

public class BulletLogic : MonoBehaviourPun
{
    public float speed; // Speed of the bullet
    public float range; // Range of the bullet
    public float damage; // Damage dealt by the bullet
    public Vector2 positionOfImpact; // Position of impact for the bullet in the case of non range based bullets
    private int shooterViewID;
    private float distanceTravelled; // Distance traveled by the bullet

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (photonView.InstantiationData != null && photonView.InstantiationData.Length > 0)
        {
            float spreadAngle = (float)photonView.InstantiationData[0];
            transform.Rotate(0, 0, spreadAngle); // Apply spread synced to all clients
        }

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

    public void SetShooterViewID(int viewID)
    {
        if (photonView.IsMine)
        {
            shooterViewID = viewID; // Set the shooter view ID
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
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            if (other.CompareTag("Destructible"))
            {
                other.gameObject.GetComponent<DestructibleObject>().RemoveHealth(damage); // Call the RemoveHealth function on the destructible object
                PhotonNetwork.Destroy(gameObject); // Destroy bullet
            }
            // Check if the bullet hit a player and it's not the shooter
            if (other.CompareTag("Player") && otherPhotonView != null && otherPhotonView.ViewID != shooterViewID)
            {
                // Call the ChangeHealthBy function on the player and sync it to all clients
                otherPhotonView.RPC("ChangeHealthBy", RpcTarget.AllBuffered, -damage);
                PhotonNetwork.Destroy(gameObject); // Destroy bullet
            }
        }
    }
}
