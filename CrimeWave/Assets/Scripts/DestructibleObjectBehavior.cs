using UnityEngine;
using Photon.Pun;

public class DestructibleObjectBehavior : MonoBehaviourPun
{
    public CurrencyHandler ch;
    public DestructibleObject destructibleObj;

    private float maxHealth;
    private float health;
    public void SetDestructibleType(DestructibleObject type)
    {
        destructibleObj = type;
        photonView.RPC("SetDestructibleTypeRPC", RpcTarget.All);
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

    [PunRPC]
    public void SetDestructibleTypeRPC()
    {
        // Set destructible object data available to all clients
        maxHealth = destructibleObj.maxHealth;
        health = destructibleObj.maxHealth;
        ch.money = destructibleObj.lootDropAmount;
        Sprite spr = GetComponent<SpriteRenderer>().sprite = destructibleObj.sprite;

        // Update collider according to sprite size ( we should keep all objects box colliders 2d for simplicity )
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = spr.bounds.size;

    }
}
