using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DestructibleObject : MonoBehaviourPun
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float lootDropAmount;

    private CurrencyHandler ch;
    private Slider healthBar;
    private float health;

    void Start()
    {
        ch = GetComponent<CurrencyHandler>();
        healthBar = GetComponentInChildren<Slider>();

        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);

        ch.money = lootDropAmount;
    }

    public void RemoveHealth(float amount)
    {
        photonView.RPC("RemoveHealthRPC", RpcTarget.All, amount);
    }

    public void DropLoot()
    {
        ch.GenerateLoot(ch.money);
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
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
