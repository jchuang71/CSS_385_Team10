using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    public int money;
    private GameObject moneyPrefab;

    public void Start()
    {
        moneyPrefab = Resources.Load<GameObject>("Prefabs/MoneyStack");
    }
    public void GiveMoney(CurrencyHandler receiver, int amount)
    {
        receiver.money += amount;
        money -= amount;
    }

    public void GenerateLoot(int amount)
    {
        //Instantiate the money prefab at the loot's position
        GameObject moneyInstance = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
        // Set the money amount designated by amount to the prefab using GiveMoney()
        GiveMoney(moneyInstance.GetComponent<CurrencyHandler>(), amount);
    }
}
