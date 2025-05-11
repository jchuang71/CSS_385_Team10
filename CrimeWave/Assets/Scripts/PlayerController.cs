using System.Collections;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CurrencyHandler ch;
    private UIManager uiManager;
    private Camera cam; // Reference to the camera
    public AudioSource soundEffects; // Reference to the audio source for player sounds
    public AudioClip collectMoney; // Reference to the audio clip for collecting money
    public float moveSpeed = 5f;
    private bool isWalking = false; // Flag to check if the player is walking for sprite animation
    private float maxHealth = 100f;
    private float health;
    private int moneyDroppedOnDeath = 10000; // money the player will drop as loot
    private float respawnDelay = 3f; // Delay before respawn
    private float respawnImmunityCurrentTime = 0f; // Current time since last immunity
    private float respawnImmunityRequiredTime = 5f; // Delay before immunity after respawn
    private bool isImmune = false; // Flag to check if the player is iummune
    private bool isDead = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ch = GetComponent<CurrencyHandler>(); // Get the CurrencyHandler component attached to the player

        health = maxHealth;

        StartCoroutine(AssignCameraWhenReady()); // Start the coroutine to assign the camera when it's ready
        uiManager = UIManager.UIManagerInstance; // Get the UIManager instance

        respawnImmunityCurrentTime = 0f; // Initialize immunity time so that players are immune at the start
        photonView.RPC("SetIsImmune", RpcTarget.All, true); // Set the immune flag to true for all players
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
            UpdateImmunity(); // Call the UpdateImmunityVisual function to update immunity visual
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
        //SUICIDE BUTTON FOR TESTING PURPOSES.
        if(Input.GetKey(KeyCode.Space)) 
        {
            Death(); //suicide
        }

        // Move the player based on the combined movement direction
        if (moveDirection != Vector2.zero)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime); // Move in the combined direction
            isWalking = true; // Set walking flag to true
        }
    }

    [PunRPC]
    public void ChangeHealthBy(float amount)
    {
        if (isDead) return; // Ignore if already dead
        if (isImmune) return; // Ignore if within immunity time

        health += amount;
        if (health <= 0)
        {
            Death(); // Call death function if health is 0 or less
        }

        // Only assign the UI if this is the local player
        if(photonView.IsMine)
        {
            uiManager.SetHealthText(health);  //Update health text
        }
    }

    [PunRPC]
    private void SetAliveState(bool isAlive)
    {
        Debug.Log("SetAliveState called with isAlive: " + isAlive);
        // Disable player visuals & components on ALL clients
        gameObject.GetComponent<SpriteRenderer>().enabled = isAlive;
        rb.simulated = isAlive; // Disable physics
        GetComponent<Collider2D>().enabled = isAlive;
        GetComponent<PlayerGun>().enabled = isAlive;
        GetComponent<CrosshairController>().enabled = isAlive;
    }

    void Death()
    {
        if (isDead) return; // Ignore if already dead

        isDead = true;
        Debug.Log("Player is dead!");

        // Call death on ALL clients
        photonView.RPC("SetAliveState", RpcTarget.All, false);

        // Call loot drop function
        ch.GenerateLoot(moneyDroppedOnDeath);
        Debug.Log("Loot dropped: " + moneyDroppedOnDeath);

        // Only the owner starts respawn logic
        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        Debug.Log("Respawning player...");

        // Update UI health text
        uiManager.SetHealthText(0);

        yield return new WaitForSeconds(respawnDelay);

        // Pick a random spawn point
        transform.position = new Vector2(0,0);

        // Reset health
        health = maxHealth;
        isDead = false;

        photonView.RPC("SetAliveState", RpcTarget.All, true);

        // Update UI health text
        uiManager.SetHealthText(health);

        // Reset immunity time
        photonView.RPC("SetIsImmune", RpcTarget.All, true);
    }

    private void UpdateImmunity()
    {
        respawnImmunityCurrentTime += Time.deltaTime;
        float alpha;
        Color color = sr.color;

        // check if player should be immune
        if(respawnImmunityCurrentTime < respawnImmunityRequiredTime)
        {
            // Flicker between transparent and opaque
            alpha = Mathf.PingPong(Time.time * 5f, 0.5f) + 0.5f; // value between 0.5 and 1
            photonView.RPC("SetSpriteAlpha", RpcTarget.All, alpha);
        }
        // player shouldnt be immune
        else
        {
            if(isImmune)
            {
                photonView.RPC("SetIsImmune", RpcTarget.All, false);
                photonView.RPC("SetSpriteAlpha", RpcTarget.All, 1f);
                return; // All immunty time has passed and flag toggled to correct state, exit the function
            }
            else
            {
                return; // All immunty time has passed and flag was already toggeled, exit the function
            }
        }
    }

    [PunRPC]
    private void SetSpriteAlpha(float alpha)
    {
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    [PunRPC]
    private void SetIsImmune(bool isImmune = true)
    {
        this.isImmune = isImmune; // Set the immune flag
        if(isImmune)
        {
            respawnImmunityCurrentTime = 0f; // Reset immunity time
        }
        else
        {
            respawnImmunityCurrentTime = respawnImmunityRequiredTime; // Set immunity time to reached potentional
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Money"))
        {
            // Only process pickup if this is the local player
            if (photonView.IsMine)
            {
                // Play collecting money sound
                soundEffects.clip = collectMoney;
                soundEffects.Play();

                // Get the MoneyLoot script from the other object
                MoneyLoot loot = other.GetComponent<MoneyLoot>();
                if (loot != null)
                {
                    // Send pickup request only to MasterClient
                    loot.photonView.RPC("RequestPickup", RpcTarget.MasterClient, photonView.ViewID);
                }
            }
        }
    }
}