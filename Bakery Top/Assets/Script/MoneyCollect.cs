using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";
    [SerializeField] public BoxCollider collider;

    private bool hasHitGround = false; // Kontrolliert, ob das Objekt den Boden berÅErt hat

    private void Start()
    {
        animator.enabled = false; // Animator standardm‰ﬂig deaktivieren
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

    private void CollectMoney()
    {
        if (animator != null)
        {
            animator.SetTrigger(collectAnimationTrigger);
        }
        Destroy(gameObject, 1.0f); // Objekt nach 1 Sekunde zerstˆren
    }
}
