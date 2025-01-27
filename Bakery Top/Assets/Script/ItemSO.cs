using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itenName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public AttributesToChange attributeToChange = new AttributesToChange();
    public int amountToChangeAttribute;

    public void SellItem()
    {
        if(statToChange == StatToChange.money)
        {
            GameObject.Find("Player").GetComponent<Player>().addMoney(amountToChangeStat);
        }
    }

    public enum StatToChange
    {
        none,
        health,
        money
    }

    public enum AttributesToChange
    {
        none,
        exp
    }
}
