using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player Stats")]
    public int playerMoney;


    public void addMoney(int amount)
    {
        playerMoney += amount;
    }
}
