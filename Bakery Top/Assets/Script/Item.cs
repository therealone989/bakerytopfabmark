using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{

    [SerializeField]
    public string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite[] itemSprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    [SerializeField] private ItemSO itemData;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void PickUp()
    {
        int leftOverItems = inventoryManager.AddItem(itemName, quantity, itemSprite, itemDescription);
        if(leftOverItems <= 0)
        {
            // Zerstört das Objekt nach dem Aufheben
            Destroy(gameObject);
        } else
        {
            // TODO: INVENAR VOLL POPUP!!!!!!!!!!!!
            quantity = leftOverItems;
            
        }
    }

    public void Interact()
    {
        PickUp();
    }

    // Text fE UI-Anzeige
    public string GetInteractText()
    {
        return $"DrEke [E], um {itemName} aufzuheben";
    }

    public string GetItemName()
    {
        return this.itemName;
    }

    public ItemSO GetItemData()
    {
        return itemData;
    }
}
