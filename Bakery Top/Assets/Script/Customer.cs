using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private ItemSeller itemSeller;
    [SerializeField] public GameObject moneyPrefab;
    [SerializeField] public Transform moneyspawnPoint;
    [SerializeField] public float throwForce = 5f;
    private InventoryManager iM;


    private void Start()
    {
        iM = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleItem(collision);
    }


    public void HandleItem(Collision collision)
    {
        Item item = collision.collider.GetComponent<Item>();
        if (item != null)
        {
            // Ruft das ItemSO vom Item-Skript ab
            ItemSO itemData = item.GetItemData();

            if (itemData != null)
            {
                // ‹bergibt das Item zum Verkauf an den ItemSeller
                itemSeller.SellItem(itemData);

                // Zerstˆrt das Objekt nach dem Verkauf
                Destroy(collision.collider.gameObject);

                StartCoroutine(ThrowMoneyWithDelay(2f));
            }
            else
            {
                Debug.LogWarning("Das Item hat keine gÅEtigen Daten und kann nicht verkauft werden.");
            }
        }
    }
    private IEnumerator ThrowMoneyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if(moneyPrefab != null && moneyspawnPoint != null)
        {
            GameObject money = Instantiate(moneyPrefab, moneyspawnPoint.position, Quaternion.identity);
            
            Rigidbody rb = money.GetComponent<Rigidbody>();
            if(rb != null)
            {
                Vector3 throwDirection = (moneyspawnPoint.forward + Vector3.up).normalized;
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("MoneyPrefab oder MoneySpawnPoint ist nicht zugewiesen.");
            }
        }
    }
}
