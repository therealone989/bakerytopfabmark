using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int playerMoney;
    [SerializeField] private TMP_Text counter;

    public int PlayerMoney => playerMoney;



    public void AddMoney(int amount)
    {
        playerMoney += amount;
        counter.text = playerMoney.ToString();
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            Debug.Log($"Geld ausgegeben: {amount}. Aktueller Kontostand: {playerMoney}");
            return true;
        }
        else
        {
            Debug.Log("Nicht genug Geld!");
            return false;
        }
    }
}
