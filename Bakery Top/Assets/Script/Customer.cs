using UnityEngine;

public class Customer : MonoBehaviour
{
    private Player player;
    private InventoryManager iM;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        iM = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item item = collision.collider.GetComponent<Item>();
        if(item != null)
        {
            string itemName = item.GetItemName();
            iM.SellItem(itemName);
            Destroy(collision.collider.gameObject);

        }
    }

}
