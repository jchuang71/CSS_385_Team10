using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;

public class DestructibleObject : MonoBehaviourPun
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float lootDropAmount;
    [SerializeField] private float respawnDelay;
    [SerializeField] private string prefabName;
    private Slider healthBar;
    private float health;

    void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
    }
    
    void Start()
    {
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

            if (PhotonNetwork.IsMasterClient)
            {
                // Call manager to respawn object after some time has passed
                DestructibleObjectManager.Instance.RespawnDestructible(prefabName, transform.position, respawnDelay);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
