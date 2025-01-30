using System.Collections;
using UnityEngine;

public class OvenSystem : MonoBehaviour, IInteractable
{
    [Header("Oven Components")]
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

    public string GetInteractText()
    {
        return "Drücke [F], um den Ofen zu benutzen.";
    }

    public void Interact()
    {
        if (firewoodCount > 0 && doughCount > 0 && isOvenClosed && !isBaking)
        {
            StartCoroutine(BakeBread());
        }
        else
        {
            Debug.Log("Der Ofen kann nicht gestartet werden! Stelle sicher, dass er geschlossen ist und Holz & Teig drin sind.");
        }
    }

    public void ToggleOvenDoor()
    {
        ToggleDoor(ovenDoorAnimator);
    }

    public void ToggleFirewoodDoor()
    {
        ToggleDoor(firewoodDoorAnimator);
    }

    private void ToggleDoor(Animator doorAnimator)
    {
        bool currentState = doorAnimator.GetBool("IsOpen");
        doorAnimator.SetBool("IsOpen", !currentState);
        CheckOvenClosedState();
    }

    private void CheckOvenClosedState()
    {
        isOvenClosed = !ovenDoorAnimator.GetBool("IsOpen") && !firewoodDoorAnimator.GetBool("IsOpen");
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>(); // Prüfen, ob das Objekt ein Item ist

        if (item != null)
        {
            string itemName = item.GetItemName(); // Den Namen des Items holen

            if (itemName == "Holz" && firewoodCount < firewoodSlots.Length)
            {
                PlaceItem(other.gameObject, firewoodSlots[firewoodCount]);
                firewoodCount++;
            }
            else if (itemName == "Teig" && doughCount < doughSlots.Length)
            {
                PlaceItem(other.gameObject, doughSlots[doughCount]);
                doughCount++;
            }
        }
    }

    private void PlaceItem(GameObject item, Transform slot)
    {
        Destroy(item);
        GameObject placeholder = new GameObject("Placeholder");
        placeholder.transform.position = slot.position;
        placeholder.transform.parent = slot;
    }

    private IEnumerator BakeBread()
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
