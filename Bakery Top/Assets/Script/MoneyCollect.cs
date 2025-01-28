using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";
    [SerializeField] private string idleAnimationTrigger = "Idle";
    private BoxCollider collider;

    private bool hasHitGround = false; // Kontrolliert, ob das Objekt den Boden ber・rt hat
    private ItemSO itemData; // Referenz auf die Item-Daten

    private void Start()
    {
        animator.enabled = false; // Animator standardm葹ig deaktivieren
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

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("HIT PLAYER");

            collider.isTrigger = true;
            ActivateAnimator();
            CollectMoney();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
            collider.isTrigger = true;
        }

        if (other.CompareTag("Player"))
        {
            GameObject emptyObject = new GameObject("MyEmptyObject");
            Debug.Log("HIT PLAYER");

            // Setze die Position des leeren Objekts auf die Position des Spielers
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            emptyObject.transform.position = playerTransform.position;

            transform.parent = emptyObject.transform;

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
            // Hier f・st du das Geld dem Spieler hinzu (durch den ItemSeller)
            ItemSeller itemSeller = FindFirstObjectByType<ItemSeller>();
            if (itemSeller != null)
            {
                Debug.Log("Itemdata nix nul");
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
