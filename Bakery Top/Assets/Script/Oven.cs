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
    public Collider firewoodTrigger; // Collider für die Holzaufnahme
    public Collider doughTrigger; // Collider für die Teigaufnahme
    public GameObject[] firewoodPlaceholders; // Platzhalter für Holzstücke
    public GameObject[] doughPlaceholders; // Platzhalter für Teigstücke

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
        Item item = other.GetComponent<Item>(); // Holt das Item-Skript vom getroffenen Objekt

        if (item != null)
        {
            string itemName = item.GetItemName();
            Debug.Log($"Erkanntes Item: {itemName}"); // Debugging

            if (other.CompareTag("Holz") && firewoodCount < firewoodSlots.Length)
            {
                Debug.Log("Holz erkannt und wird platziert!"); // Debugging
                PlaceItem(other.gameObject, firewoodSlots[firewoodCount]);
                firewoodPlaceholders[firewoodCount].SetActive(true); // Aktiviert den Platzhalter für Holz
                firewoodCount++;
            }
            else if (other.CompareTag("Dough") && doughCount < doughSlots.Length)
            {
                Debug.Log("Teig erkannt und wird platziert!"); // Debugging
                PlaceItem(other.gameObject, doughSlots[doughCount]);
                doughPlaceholders[doughCount].SetActive(true); // Aktiviert den Platzhalter für Teig
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

        // Platzhalter zurücksetzen
        foreach (GameObject placeholder in firewoodPlaceholders)
        {
            placeholder.SetActive(false);
        }
        foreach (GameObject placeholder in doughPlaceholders)
        {
            placeholder.SetActive(false);
        }

        firewoodCount = 0;
        doughCount = 0;
        isBaking = false;
    }
}
