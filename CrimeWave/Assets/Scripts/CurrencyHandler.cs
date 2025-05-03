using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    public int money;

    public void GiveMoney(CurrencyHandler receiver, int amount)
    {
        receiver.money += amount;
        money -= amount;
    }
}
