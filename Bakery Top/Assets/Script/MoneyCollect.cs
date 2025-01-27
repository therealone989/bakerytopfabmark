using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string collectAnimationTrigger = "Collect";

    private bool hashitGround = false;
        private void Start()
    {
        animator.enabled = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!hashitGround && collision.collider.CompareTag("Ground"))
        {
            hashitGround = true;
            ActivateAnimator();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            CollectMoney();
        }
    }

    private void ActivateAnimator()
    {
        if(animator != null)
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
        Destroy(gameObject, 1.0f);
    }
}
