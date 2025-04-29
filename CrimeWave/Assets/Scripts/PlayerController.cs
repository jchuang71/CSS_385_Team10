using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private float maxHealth = 100f;
    private float health;
    [SerializeField] private float money;

    private Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
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
    }

    private void FixedUpdate()
    {
        // Only control this player if it's ours
        if (photonView.IsMine)
        {
            // Get input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Calculate movement
            Vector2 movement = new Vector2(horizontal, vertical) * moveSpeed * Time.fixedDeltaTime;

            // Apply movement
            rb.MovePosition(rb.position + movement);

            // Debug logging for movement
            if (horizontal != 0 || vertical != 0)
            {
                Debug.Log($"Moving player: h={horizontal}, v={vertical}");
            }
        }
    }

    public void AddHealth(float amount)
    {
        health += amount;
    }

    public void AddMoney(float amount)
    {
        money += amount;
    }
}