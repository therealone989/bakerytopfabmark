using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;


    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inventoryManager.AddItem(itemName, quantity);
            Destroy(gameObject);
        }
    }
}
