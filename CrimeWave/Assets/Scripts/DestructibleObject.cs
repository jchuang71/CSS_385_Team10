using UnityEngine;
using Photon.Pun;

public class DestructibleObject : MonoBehaviourPun
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float lootDropAmount;

    private CurrencyHandler ch;
    private float health;

    void Start()
    {
        ch = GetComponent<CurrencyHandler>();

        health = maxHealth;
        ch.money = lootDropAmount;
    }
    public void AddHealth(float amount)
    {
        health += amount;
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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a * 0.75f);

        if (health <= 0)
        {
            DropLoot(); // Call loot drop function
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
