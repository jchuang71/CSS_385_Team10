using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UIManagerInstance;// Singleton instance of UIManager
    public TMP_Text healthText; // Reference to the health text UI element
    public TMP_Text moneyText; // Reference to the money text UI element
    public Image weaponSelectorImage; // Reference to the weapon selector image UI element

    void Awake()
    {
        if (UIManagerInstance == null)
        {
            UIManagerInstance = this;
        }
        else if (UIManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Only assign the UI if this is the local player. Thus no need for photonView.
        healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
        moneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        weaponSelectorImage = GameObject.Find("WeaponSelected").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealthText(float health)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString("F0"); // Update the health text with 0 decimal places
        }
        else
        {
            Debug.LogError("healthText is not assigned in the inspector!"); // Error message if healthText is not assigned
        }
    }

    public void SetMoneyText(float money)
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + money.ToString("F0"); // Update the money text with 0 decimal places
        }
        else
        {
            Debug.LogError("moneyText is not assigned in the inspector!"); // Error message if moneyText is not assigned
        }
    }

    public void SetWeaponSelectorImage(Sprite sprite)
    {
        if (weaponSelectorImage != null)
        {
            weaponSelectorImage.sprite = sprite; // Update the weapon selector image with the new sprite
        }
        else
        {
            Debug.LogError("weaponSelectorImage is not assigned in the inspector!"); // Error message if weaponSelectorImage is not assigned
        }
    }
}
