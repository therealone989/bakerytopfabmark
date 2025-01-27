using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";

    private bool hasHitGround = false; // Kontrolliert, ob das Objekt den Boden berührt hat

    private void Start()
    {
        animator.enabled = false; // Animator standardmäßig deaktivieren
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
        }

        if (collision.gameObject.CompareTag("Player"))
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
        Destroy(gameObject, 1.0f); // Objekt nach 1 Sekunde zerstören
    }
}
