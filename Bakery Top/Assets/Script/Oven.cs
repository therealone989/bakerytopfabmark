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
    public Collider fireWoodTrigger;
    public Collider doughTrigger;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Linke Maustaste für Interaktion
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 3f)) // Reichweite 3m
            {
                if (hit.collider.CompareTag("OvenDoor")) // Ofentür öffnen/schließen
                {
                    ToggleOvenDoor();
                }
                else if (hit.collider.CompareTag("FirewoodDoor")) // Brennholztür öffnen/schließen
                {
                    ToggleFirewoodDoor();
                }
            }
        }
    }
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
        Debug.Log($"Objekt {other.name} ist in den Trigger eingetreten!"); // Debugging

        Item item = other.GetComponent<Item>(); // Holt das Item-Skript vom getroffenen Objekt

        if (item != null)
        {
            string itemName = item.GetItemName();
            Debug.Log($"Erkanntes Item: {itemName}"); // Debugging

            if (other.gameObject.CompareTag("Holz") && firewoodCount < firewoodSlots.Length)
            {
                Debug.Log("Holz erkannt und wird platziert!"); // Debugging
                PlaceItem(other.gameObject, firewoodSlots[firewoodCount]);
                firewoodCount++;
            }
            else if (other.gameObject.CompareTag("Teig") && doughCount < doughSlots.Length)
            {
                Debug.Log("Teig erkannt und wird platziert!"); // Debugging
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
