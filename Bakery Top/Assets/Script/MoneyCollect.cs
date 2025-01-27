using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";
    private BoxCollider collider;

    private bool hasHitGround = false; // Kontrolliert, ob das Objekt den Boden berührt hat
    private ItemSO itemData; // Referenz auf die Item-Daten

    private void Start()
    {
        animator.enabled = false; // Animator standardmäßig deaktivieren
        collider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitGround && collision.collider.CompareTag("Ground"))
        {
            hasHitGround = true;

            GameObject emptyObject = new GameObject("MyEmptyObject");
            emptyObject.transform.position = this.transform.position;

            transform.parent = emptyObject.transform;

            ActivateAnimator();
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollectMoney();
        }
    }

    private void ActivateAnimator()
    {
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    public void SetItemData(ItemSO itemData)
    {
        this.itemData = itemData; // Speichern der Item-Daten
    }

    private void CollectMoney()
    {
        if (itemData != null)
        {
            // Hier fügst du das Geld dem Spieler hinzu (durch den ItemSeller)
            ItemSeller itemSeller = FindFirstObjectByType<ItemSeller>();
            if (itemSeller != null)
            {
                itemSeller.SellItem(itemData); // Füge das Geld dem Spieler hinzu
            }
        }

        // Animation auslösen
        if (animator != null)
        {
            animator.SetTrigger(collectAnimationTrigger);
        }

        // Objekt nach 1 Sekunde zerstören
        Destroy(gameObject, 1.0f);
    }
}
