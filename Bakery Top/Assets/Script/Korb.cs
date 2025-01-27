using UnityEngine;

public class Korb : MonoBehaviour
{
    public GameObject[] basketBreads;
    public GameObject breadPrefab;
    public Transform playerHand;
    private int currentBreadCount = 0;
    public float raycastRange = 2f;
    public Transform playerCamera;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Pan"))
        {
            Destroy(collision.gameObject);

            if (currentBreadCount < basketBreads.Length)
            {
                basketBreads[currentBreadCount].SetActive(true);

                currentBreadCount++;
            }
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForBasket();
        }
    }
    private void CheckForBasket()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, raycastRange))
        {
            if(hit.collider.gameObject == this.gameObject)
            {
                TryGiveBreadToPlayer();
            }
        }
    }

    private void TryGiveBreadToPlayer()
    {

        if (currentBreadCount > 0)
        {

            basketBreads[currentBreadCount - 1].SetActive(false);
            currentBreadCount--;

            GameObject bread = Instantiate(breadPrefab, playerHand.position, Quaternion.identity);
            bread.transform.SetParent(playerHand);

            Grabitem grabItem = playerHand.GetComponentInParent<Grabitem>();
            if (grabItem != null)
            {
                grabItem.GrabObject(bread);
            }
        }
    }
}
