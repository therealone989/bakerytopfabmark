using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";
    [SerializeField] private string idleAnimationTrigger = "Idle";
    private BoxCollider collider;
    private Rigidbody body;

    private bool hasHitGround = false; // Kontrolliert, ob das Objekt den Boden ber・rt hat
    private ItemSO itemData; // Referenz auf die Item-Daten

    private void Start()
    {
        animator.enabled = false; // Animator standardm葹ig deaktivieren
        collider = GetComponent<BoxCollider>();
        body = GetComponent<Rigidbody>();
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

        if (collision.gameObject.CompareTag("Player"))
        {
            collider.isTrigger = true;
            ActivateAnimator();
            PlayCollectAnimation();
            CollectMoney();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHitGround && other.CompareTag("Ground"))
        {
            hasHitGround = true;

            GameObject emptyObject = new GameObject("MyEmptyObject");
            emptyObject.transform.position = this.transform.position;

            transform.parent = emptyObject.transform;

            ActivateAnimator();
            collider.isTrigger = true;
        }

        if (other.CompareTag("Player"))
        {
            ActivateAnimator();
            PlayCollectAnimation();
            CollectMoney();
        }
    }
    private void PlayCollectAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Collect"); // Setzt den Trigger für die Collect-Animation
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
            // Hier f・st du das Geld dem Spieler hinzu (durch den ItemSeller)
            ItemSeller itemSeller = FindFirstObjectByType<ItemSeller>();
            if (itemSeller != null)
            {
                itemSeller.SellItem(itemData); // F・e das Geld dem Spieler hinzu
            }
        }

        // Animation auslen
        if (animator != null)
        {
            animator.SetTrigger(collectAnimationTrigger);
        }

        // Objekt nach 1 Sekunde zersten
        Destroy(gameObject, 1.0f);
    }
}
