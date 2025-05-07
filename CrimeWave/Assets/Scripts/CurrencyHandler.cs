using Photon.Pun;
using UnityEngine;

public class CurrencyHandler : MonoBehaviourPun
{
    public float money;
    private GameObject moneyPrefab;

    public void Start()
    {
        moneyPrefab = Resources.Load<GameObject>("Prefabs/MoneyStack");
    }

    public void GiveMoney(int photonId, float amount)
    {
        photonView.RPC("AddMoney", RpcTarget.All, new object[] { photonId, amount });
    }

    public void GenerateLoot(float amount)
    {
        //Instantiate the money prefab at the loot's position
        GameObject moneyInstance = PhotonNetwork.InstantiateRoomObject("Prefabs/" + moneyPrefab.name, transform.position, Quaternion.identity);
        // Set the money amount designated by amount to the prefab using GiveMoney()
        GiveMoney(moneyInstance.GetComponent<PhotonView>().ViewID, amount);
    }

    [PunRPC]
    public void AddMoney(int photonId, float amount)
    {
        // other object
        PhotonView.Find(photonId).GetComponent<CurrencyHandler>().money += amount;

        // this object
        PhotonView.Find(photonView.ViewID).GetComponent<CurrencyHandler>().money -= amount;
    }

}
