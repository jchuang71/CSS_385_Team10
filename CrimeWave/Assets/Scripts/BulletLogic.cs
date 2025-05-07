using UnityEngine;
using Photon.Pun;

public class BulletLogic : MonoBehaviourPun
{
    public float speed; // Speed of the bullet
    public float range; // Range of the bullet
    public float damage; // Damage dealt by the bullet
    public float splashDamageRange; // Splash damage range of bullet on hit or impact
    private int shooterViewID;
    private float distanceTravelled; // Distance traveled by the bullet
    private bool hasHit = false; // Flag to check if the bullet has hit something

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
            splashDamageRange = gun.bulletSplashDamageRange; // Set the bullet splash damage range from the gun data
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
                BulletFinished(); // Call the BulletFinished function if the bullet exceeds its range
            }
        }
    }

    void BulletFinished()
    {
        //finds all colliders in the area of the bullet splash damage. Direct hitting gun will have splashDamageRange 0.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashDamageRange);

        foreach (Collider2D hit in hitColliders)
        {
            PhotonView hitPV = hit.GetComponent<PhotonView>();

            Debug.Log("Hit: " + hit.name); // Debug log to check what the bullet hit

            // Damage other players (but not the shooter)
            if (hit.CompareTag("Player") && hitPV != null && hitPV.ViewID != shooterViewID)
            {
                hitPV.RPC("ChangeHealthBy", RpcTarget.AllBuffered, -damage);
            }

            // Damage destructible objects
            if (hit.CompareTag("Destructible"))
            {
                hit.GetComponent<DestructibleObject>().RemoveHealth(damage);
            }
        }
        PhotonNetwork.Destroy(gameObject); // Destroy bullet
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine || hasHit) return; // skip if not mine or already hit
        PhotonView otherPV = other.GetComponent<PhotonView>();
        if (other.CompareTag("Player") && otherPV != null && otherPV.ViewID != shooterViewID)
        {
            BulletFinished(); // Call the BulletFinished function if it hits a player
            hasHit = true; // mark as hit so we don't double hit
        }
        if (other.CompareTag("Destructible"))
        {
            BulletFinished(); // Call the BulletFinished function if it hits a destructible object
            hasHit = true; // mark as hit so we don't double hit
        }
        // otherwise hit some other object and dont do anything
    }
}
