using UnityEngine;
using Photon.Pun;
using System.Collections;

public class CrosshairController : MonoBehaviourPun
{
    [SerializeField] private Sprite crosshairSprite; // Reference to the crosshair sprite
    private GameObject crosshairObject; // The active crosshair game object
    private Camera playerCamera; // Reference to the main camera

    void Start()
    {
        // Only create crosshair for the local player
        if (photonView.IsMine)
        {
            // Hide the system cursor
            Cursor.visible = false;

            // Start looking for the camera and create crosshair when ready
            StartCoroutine(WaitForCamera());
        }
    }

    private IEnumerator WaitForCamera()
    {
        // Wait until the camera is available
        while (Camera.main == null)
        {
            yield return null;
        }
        playerCamera = Camera.main;

        // Create the crosshair once we have the camera
        SpawnCrosshair();
    }

    void Update()
    {
        // Only update crosshair for the local player
        if (photonView.IsMine && crosshairObject != null && playerCamera != null)
        {
            UpdateCrosshairPosition();
        }
    }

    void SpawnCrosshair()
    {
        if (crosshairSprite != null && playerCamera != null)
        {
            // Create a new game object for the crosshair
            crosshairObject = new GameObject("PlayerCrosshair");

            // Add a Sprite Renderer component
            SpriteRenderer spriteRenderer = crosshairObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = crosshairSprite;

            // Make sure the crosshair is visible by setting a high sorting layer
            spriteRenderer.sortingOrder = 999;

            // Set an appropriate scale for the crosshair
            crosshairObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            // Position the crosshair initially
            UpdateCrosshairPosition();
        }
        else
        {
            Debug.LogError("Crosshair creation failed: Sprite or camera not found.");
        }
    }

    void UpdateCrosshairPosition()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Convert to world position
        mousePos.z = Mathf.Abs(playerCamera.transform.position.z);
        Vector3 worldPos = playerCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0; // Keep it in 2D plane

        // Update the crosshair position
        crosshairObject.transform.position = worldPos;
    }

    void OnDestroy()
    {
        // Show the cursor again when the player is destroyed
        if (photonView.IsMine)
        {
            Cursor.visible = true;

            // Destroy the crosshair
            if (crosshairObject != null)
            {
                Destroy(crosshairObject);
            }
        }
    }
}