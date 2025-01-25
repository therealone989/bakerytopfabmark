using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void PickUp()
    {
        inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
        Destroy(gameObject);
    }
}
