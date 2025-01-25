using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite[] sprite;

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

        // Übergibt das ausgewählte Sprite an den InventoryManager
        inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);

        // Zerstört das Objekt nach dem Aufheben
        Destroy(gameObject);
    }
}
