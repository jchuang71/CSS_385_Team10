using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private bool isWalking = false; // Flag to check if the player is walking for sprite animation
    private Rigidbody2D rb;
    private float maxHealth = 100f;
    private float health;
    public AudioSource soundEffects; // Reference to the audio source for player sounds
    public AudioClip collectMoney; // Reference to the audio clip for collecting money
    [SerializeField] private int moneyDroppedOnDeath = 1000; // money the player will drop as loot
    public CurrencyHandler ch; // Reference to the CurrencyHandler script
    [SerializeField] private Camera cam; // Reference to the camera

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        ch = GetComponent<CurrencyHandler>(); // Get the CurrencyHandler component attached to the player

        StartCoroutine(AssignCameraWhenReady()); // Start the coroutine to assign the camera when it's ready
    }

    void Update()
    {
        if (photonView == null)
        {
            Debug.LogError("PhotonView is null on PlayerController!");
            return;
        }

        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
        }

        // Update the health and money text UI to 0 decimal places
        //healthText.text = "+ " + health.ToString("F0");
        //moneyText.text = "$ " + ch.money.ToString("F0");
    }

    private void FixedUpdate()
    {
        // Only control this player if it's ours
        if (photonView.IsMine)
        {
            MovePlayer();
            FaceCursor(); // Call the FaceCursor function to face the cursor
        }
    }



    private IEnumerator AssignCameraWhenReady()
    {
        // Keep checking until Camera.main is ready (not null)
        while (Camera.main == null)
        {
            yield return null; // Wait for next frame
        }

        cam = Camera.main;
        Debug.Log("Camera assigned: " + cam.name);
    }

    private void FaceCursor()
    {
        if (cam == null)
        {
//            Debug.LogError("Camera is not assigned or instantiated!");
            return; // Exit if camera is null
        }
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 mousePos = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log("mouse position " + mousePos + " direction " + direction + " angle " + angle); // Debug log to check if the function is called
        rb.rotation = angle-90; // Rotate the player to face the mouse position
    }

    private void MovePlayer()
    {
        isWalking = false; // Reset the walking flag to false

        // Initialize movement vector
        Vector2 moveDirection = Vector2.zero;

        // Add movement direction based on input keys
        if (Input.GetKey(KeyCode.W)) 
        {
            moveDirection += Vector2.up; // Move up
        }
        if (Input.GetKey(KeyCode.A)) 
        {
            moveDirection += Vector2.left; // Move left
        }
        if (Input.GetKey(KeyCode.S)) 
        {
            moveDirection += Vector2.down; // Move down
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            moveDirection += Vector2.right; // Move right
        }

        // Move the player based on the combined movement direction
        if (moveDirection != Vector2.zero)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime); // Move in the combined direction
            isWalking = true; // Set walking flag to true
        }
    }

    public void ChangeHealthBy(float amount)
    {
        if (photonView.IsMine)
        {
            health += amount;
            if(health <= 0)
            {
                // Handle player death here, e.g., respawn or game over
                Debug.Log("Player is dead!");
                health = maxHealth; // Reset health for respawn
                // Call loot drop function
                ch.GenerateLoot(moneyDroppedOnDeath);
            }
            // Only assign the UI if this is the local player

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Money")
        {
            //Play collecting money sound
            soundEffects.clip = collectMoney;
            soundEffects.Play();
            // Display money gained over item through screen space canvas
            // Add the money to the player's currency
            other.GetComponent<CurrencyHandler>().GiveMoney(photonView.ViewID, other.GetComponent<CurrencyHandler>().money);
            // Destroy the money object after picking it up
            PhotonNetwork.Destroy(other.gameObject);
        }
    }
}