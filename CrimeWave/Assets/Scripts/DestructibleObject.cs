using UnityEngine;
using Photon.Pun;

public class DestructibleObject : MonoBehaviourPun
{
    public float maxHealth = 20f;
    public float health;
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
        // Generates spawned money in a range of 20% below or above the designated amount
        int spawnedMoney = (int)UnityEngine.Random.Range(ch.money * 0.8f, ch.money * 1.2f);
        ch.GenerateLoot(spawnedMoney);
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
