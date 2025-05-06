using UnityEngine;
using Photon.Pun;

public class DestructibleObject : MonoBehaviourPun
{
    private float maxHealth = 20f;
    private float health;
    public CurrencyHandler ch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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

/*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().AddMoney(20);
            PhotonNetwork.Destroy(gameObject);
        }
    }
    */
}
