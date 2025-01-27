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
                // Übergibt das Item zum Verkauf an den ItemSeller
                itemSeller.SellItem(itemData);

                // Zerstört das Objekt nach dem Verkauf
                Destroy(collision.collider.gameObject);
            }
            else
            {
                Debug.LogWarning("Das Item hat keine gültigen Daten und kann nicht verkauft werden.");
            }

        }
    }

}
