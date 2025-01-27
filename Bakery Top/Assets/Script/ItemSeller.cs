using UnityEngine;

public class ItemSeller : MonoBehaviour
{
    [SerializeField]
    private MoneyManager moneyManager;
    public void SellItem(ItemSO itemSO)
    {
        if (itemSO.statToChange == ItemSO.StatToChange.money)
        {
            moneyManager.AddMoney(itemSO.amountToChangeStat);
            Debug.Log($"Item verkauft: {itemSO.itenName}. Spieler erhält {itemSO.amountToChangeStat} Münzen.");
        }
        else
        {
            Debug.Log($"Item {itemSO.itenName} hat keinen Verkaufswert.");
        }
    }
}
