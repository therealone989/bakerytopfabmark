using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private ItemSeller itemSeller;
    private InventoryManager iM;

    private void Start()
    {
        iM = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item item = collision.collider.GetComponent<Item>();
        if(item != null)
        {
            // Ruft das ItemSO vom Item-Skript ab
            ItemSO itemData = item.GetItemData();

            if (itemData != null)
            {
                // �bergibt das Item zum Verkauf an den ItemSeller
                itemSeller.SellItem(itemData);

                // Zerst�rt das Objekt nach dem Verkauf
                Destroy(collision.collider.gameObject);
            }
            else
            {
                Debug.LogWarning("Das Item hat keine g�ltigen Daten und kann nicht verkauft werden.");
            }

        }
    }

}
