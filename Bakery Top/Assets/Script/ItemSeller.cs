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
            Debug.Log($"Item verkauft: {itemSO.itemName}. Spieler erh‰lt {itemSO.amountToChangeStat} MÅEzen.");
        }
        else
        {
            Debug.Log($"Item {itemSO.itemName} hat keinen Verkaufswert.");
        }
    }
}
