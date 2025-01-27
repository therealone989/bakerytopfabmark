using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itenName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public AttributesToChange attributeToChange = new AttributesToChange();
    public int amountToChangeAttribute;

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
