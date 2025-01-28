using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";
    [SerializeField] private string idleAnimationTrigger = "Idle";
    private BoxCollider collider;

    private bool hasHitGround = false; // Kontrolliert, ob das Objekt den Boden berührt hat
    private bool hasBeenCollected = false; // Verhindert mehrfaches Sammeln
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
            Debug.Log("HIT GROUND");
            hasHitGround = true;

            // Spawne das leere Objekt an der Position des Spielers
            GameObject emptyObject = new GameObject("MyEmptyObject");
            emptyObject.transform.position = this.transform.position; // Position des Spielers

            transform.parent = emptyObject.transform;

            ActivateAnimator();
            collider.isTrigger = true;
        }

        if (!hasBeenCollected && collision.gameObject.CompareTag("Player"))
        {
            hasBeenCollected = true; // Stelle sicher, dass das Sammeln nur einmal erfolgt
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER AUSGELÖST");
        if (!hasHitGround && other.CompareTag("Ground"))
        {
            hasHitGround = true;

            // Hole die Position des Spielers (falls erforderlich)
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            // Spawne das leere Objekt an der Position des Spielers
            GameObject emptyObject = new GameObject("MyEmptyObject");
            emptyObject.transform.position = playerTransform.position; // Position des Spielers
            transform.parent = emptyObject.transform;

            ActivateAnimator();
            CollectMoney();
            collider.isTrigger = true;
        }

        // Stelle sicher, dass das Geld nur einmal eingesammelt wird
        if (!hasBeenCollected && other.CompareTag("Player"))
        {
            GameObject emptyObject = new GameObject("MyEmptyObject");

            // Setze die Position des leeren Objekts auf die Position des Spielers
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            emptyObject.transform.position = playerTransform.position;

            transform.parent = emptyObject.transform;

            hasBeenCollected = true; // Stelle sicher, dass das Sammeln nur einmal erfolgt

            ActivateAnimator();
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
