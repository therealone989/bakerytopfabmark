using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite[] itemSprite;

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
}
