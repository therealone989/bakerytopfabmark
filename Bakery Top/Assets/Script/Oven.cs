using System.Collections;
using UnityEngine;

public class OvenSystem : MonoBehaviour
{
    public Animator ovenDoorAnimator;
    public Animator firewoodDoorAnimator;
    public Transform[] firewoodSlots;
    public Transform[] doughSlots;
    public GameObject fireEffect;
    public GameObject bakedBreadPrefab;
    public float bakingTime = 10f;

    private int firewoodCount = 0;
    private int doughCount = 0;
    private bool isOvenClosed = false;
    private bool isBaking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Linke Maustaste für Interaktion
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 4f))
            {
                if (hit.collider.CompareTag("OvenButton") && firewoodCount > 0 && doughCount > 0 && isOvenClosed && !isBaking)
                {
                    StartCoroutine(BakeBread());
                }
                else if (hit.collider.CompareTag("OvenDoor"))
                {
                    ToggleDoor(ovenDoorAnimator);
                }
                else if (hit.collider.CompareTag("FirewoodDoor"))
                {
                    ToggleDoor(firewoodDoorAnimator);
                }
            }
        }
    }

    void ToggleDoor(Animator doorAnimator)
    {
        bool currentState = doorAnimator.GetBool("IsOpen");
        doorAnimator.SetBool("IsOpen", !currentState);
        CheckOvenClosedState();
    }

    void CheckOvenClosedState()
    {
        isOvenClosed = !ovenDoorAnimator.GetBool("IsOpen") && !firewoodDoorAnimator.GetBool("IsOpen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Holz") && firewoodCount < firewoodSlots.Length)
        {
            PlaceItem(other.gameObject, firewoodSlots[firewoodCount]);
            firewoodCount++;
        }
        else if (other.CompareTag("Teig") && doughCount < doughSlots.Length)
        {
            PlaceItem(other.gameObject, doughSlots[doughCount]);
            doughCount++;
        }
    }

    void PlaceItem(GameObject item, Transform slot)
    {
        Destroy(item);
        GameObject placeholder = new GameObject("Placeholder");
        placeholder.transform.position = slot.position;
        placeholder.transform.parent = slot;
    }

    IEnumerator BakeBread()
    {
        isBaking = true;
        fireEffect.SetActive(true);
        ovenDoorAnimator.SetTrigger("BakeStart");

        yield return new WaitForSeconds(bakingTime);

        fireEffect.SetActive(false);
        ovenDoorAnimator.SetTrigger("BakeEnd");

        foreach (Transform doughSlot in doughSlots)
        {
            Instantiate(bakedBreadPrefab, doughSlot.position, Quaternion.identity);
        }

        firewoodCount = 0;
        doughCount = 0;
        isBaking = false;
    }
}
