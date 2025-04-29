using UnityEngine;
using Photon.Pun;

public class DestructibleObject : MonoBehaviourPun
{
    private float maxHealth = 20f;
    private float health;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().AddMoney(20);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
