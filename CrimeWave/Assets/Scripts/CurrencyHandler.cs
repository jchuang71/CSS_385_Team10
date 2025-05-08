using Photon.Pun;
using UnityEngine;
using System.Collections;

public class CurrencyHandler : MonoBehaviourPun
{
    public float money;
    private GameObject moneyPrefab;
    [SerializeField] public UIManager uiManager;

    private void Awake()
    {
        moneyPrefab = Resources.Load<GameObject>("Prefabs/MoneyStack");
    }

    // Called when Photon instantiates this player object
    private void Start()
    {
        // Only initialize UI for the local player
        if (photonView.IsMine)
        {
            StartCoroutine(WaitForUIManager());
        }
    }

    private IEnumerator WaitForUIManager()
    {
        // Wait until UIManager is assigned before proceeding
        while (uiManager == null)
        {
            uiManager = UIManager.UIManagerInstance;
            yield return null; // Wait for the next frame
        }
        Debug.Log("UIManager assigned successfully.");
        UpdateMoneyUI();
    }

    public void GenerateLoot(float amount)
    {
        if (amount <= 0f || amount > money)
        {
            Debug.LogWarning("Invalid loot amount.");
            return;
        }

        // Pass loot amount directly in instantiation data (cleaner than RPC)
        object[] data = new object[] { amount };
        PhotonNetwork.InstantiateRoomObject("Prefabs/MoneyStack", transform.position, Quaternion.identity, 0, data);

        ChangeMoneyBy(-amount);
    }

    // Call this when picking up loot or gaining money
    public void ChangeMoneyBy(float amount)
    {
        if (photonView.IsMine)
        {
            money += amount;
            Debug.Log("Money changed by: " + amount + " New total: " + money);
            UpdateMoneyUI();
        }
    }

    private void UpdateMoneyUI()
    {
        Debug.Log("photonView.IsMine: " + photonView.IsMine);
        Debug.Log("uiManager is null? " + (uiManager == null));
        if (photonView.IsMine && uiManager != null)
        {
            Debug.Log("Updating money UI: " + money);
            uiManager.SetMoneyText(money);
        }
    }
}
