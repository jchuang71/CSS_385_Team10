using Photon.Pun;
using UnityEngine;

public class MoneyLoot : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public float moneyAmount;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = photonView.InstantiationData;
        if (instantiationData != null && instantiationData.Length > 0)
        {
            moneyAmount = (float)instantiationData[0];
            Debug.Log("MoneyLoot initialized with amount: " + moneyAmount);
        }
        else
        {
            Debug.LogWarning("MoneyLoot instantiated without data!");
        }
    }

    [PunRPC]
    void RequestPickup(int playerViewID, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient) return; // Only Master handles pickup

        PhotonView playerPhotonView = PhotonView.Find(playerViewID);
        Debug.Log("RequestPickup called by player with ID: " + playerViewID);
        if (playerPhotonView != null)
        {
            Debug.Log("MasterClient found player with ID: " + playerViewID);
            CurrencyHandler currencyHandler = playerPhotonView.GetComponent<CurrencyHandler>();
            if (currencyHandler != null)
            {
                currencyHandler.ChangeMoneyBy(moneyAmount);
                Debug.Log("Master added " + moneyAmount + " to player with ID " + playerViewID);
            }
            else
            {
                Debug.LogWarning("CurrencyHandler not found on player with ID " + playerViewID);
            }
        }

        // MasterClient destroys the loot
        PhotonNetwork.Destroy(gameObject);
    }
}
