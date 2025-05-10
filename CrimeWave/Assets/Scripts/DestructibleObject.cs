using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DestructibleObject : MonoBehaviourPun
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float lootDropAmount;
    private Slider healthBar;
    private float health;

    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();

        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);
    }

    public void RemoveHealth(float amount)
    {
        photonView.RPC("RemoveHealthRPC", RpcTarget.All, amount);
    }

    public void DropLoot()
    {
        // Pass the loot amount via instantiationData
        object[] instantiationData = new object[] { lootDropAmount };
        PhotonNetwork.InstantiateRoomObject("Prefabs/MoneyStack", transform.position, Quaternion.identity, 0, instantiationData);
        
        Debug.Log("Dropping loot from " + gameObject.name);
    }

    [PunRPC]
    public void RemoveHealthRPC(float amount)
    {
        health -= amount;
        healthBar.value = health;

        if (health < maxHealth)
            healthBar.gameObject.SetActive(true);

        if (health <= 0)
        {
            DropLoot(); // Call loot drop function
            if(photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
