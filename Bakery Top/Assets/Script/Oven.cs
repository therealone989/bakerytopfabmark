using System.Collections;
using UnityEngine;

public class Oven : MonoBehaviour, IInteractable
{
    [Header("Oven Components")]
    public Animator ovenDoorAnimator;     // Animiert die obere Tür
    public Animator firewoodDoorAnimator; // Animiert die untere Tür
    public Transform[] firewoodSlots;     // Slots für Holz
    public Transform[] doughSlots;        // Slots für Teig
    public GameObject fireEffect;         // Feuer-Effekt während des Backens
    public GameObject bakedBreadPrefab;   // Gebackenes Brot
    public float bakingTime = 10f;        // Dauer des Backens
    public GameObject firewoodCollider;      // Collider für Feuerholz
    public GameObject doughCollider;         // Collider für Teig
    public GameObject[] firewoodPlaceholders; // Platzhalter für Holzstücke
    public GameObject[] doughPlaceholders;    // Platzhalter für Teigstücke
    public GameObject startButton;        // Startknopf für den Ofen

    private int firewoodCount = 0;  // Anzahl der eingesetzten Holzstücke
    private int doughCount = 0;     // Anzahl der eingesetzten Teigstücke
    private bool isOvenClosed = false; // Prüft, ob beide Türen geschlossen sind
    private bool isBaking = false;  // Ist der Ofen gerade am Backen?

    public string GetInteractText()
    {
        if (isBaking) return "Das Brot backt gerade...";
        if (!isOvenClosed) return "Schließe beide Türen, um den Ofen zu starten.";
        return "Drücke [E], um den Ofen zu starten.";
    }

    public void Interact()
    {
        // Prüfen, ob der Ofen bereit ist zum Starten
        if (firewoodCount > 0 && doughCount >= 4 && isOvenClosed && !isBaking)
        {
            StartCoroutine(BakeBread());
        }
        else
        {
            Debug.Log("Ofen kann nicht starten! Türen schließen und Holz & Teig einfügen.");
        }
    }

    public void ToggleOvenDoor()
    {
        Debug.Log("OBEN DOOR");
        ToggleDoor(ovenDoorAnimator);
    }

    public void ToggleFirewoodDoor()
    {
        Debug.Log("Unten DOOR");
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

    //private void OnCollisionEnter(Collision other)
    //{
    //    Item item = other.collider.GetComponent<Item>();

    //    if (item != null)
    //    {
    //        string itemName = item.GetItemName();

    //        if (itemName == "Holz" && firewoodCount < firewoodSlots.Length)
    //        {
    //            if (firewoodDoorAnimator.GetBool("IsOpen")) // Prüfen, ob die Brennholztür offen ist
    //            {
    //                PlaceItem(other.gameObject, firewoodSlots[firewoodCount]);
    //                firewoodPlaceholders[firewoodCount].SetActive(true);
    //                firewoodCount++;
    //            }
    //            else
    //            {
    //                Debug.Log("Die Brennholztür muss offen sein, um Holz einzulegen!");
    //            }
    //        }
    //        else if (itemName == "Dough" && doughCount < doughSlots.Length)
    //        {
    //            if (ovenDoorAnimator.GetBool("IsOpen")) // Prüfen, ob die Ofentür offen ist
    //            {
    //                PlaceItem(other.gameObject, doughSlots[doughCount]);
    //                doughPlaceholders[doughCount].SetActive(true);
    //                doughCount++;
    //            }
    //            else
    //            {
    //                Debug.Log("Die Ofentür muss offen sein, um Teig einzulegen!");
    //            }
    //        }
    //    }
    //}


    private void PlaceItem(GameObject item, Transform slot)
    {
        Destroy(item);
        GameObject placeholder = new GameObject("Placeholder");
        placeholder.transform.position = slot.position;
        placeholder.transform.parent = slot;
    }

    public void AddFirewood(GameObject firewood)
    {
        if (firewoodCount < firewoodSlots.Length && firewoodDoorAnimator.GetBool("IsOpen"))
        {
            PlaceItem(firewood, firewoodSlots[firewoodCount]);
            firewoodPlaceholders[firewoodCount].SetActive(true);
            firewoodCount++;
        }
        else
        {
            Debug.Log("Die Brennholztür muss offen sein, um Holz einzulegen!");
        }
    }

    public void AddDough(GameObject dough)
    {
        if (doughCount < doughSlots.Length && ovenDoorAnimator.GetBool("IsOpen"))
        {
            PlaceItem(dough, doughSlots[doughCount]);
            doughPlaceholders[doughCount].SetActive(true);
            doughCount++;
        }
        else
        {
            Debug.Log("Die Ofentür muss offen sein, um Teig einzulegen!");
        }
    }

    private IEnumerator BakeBread()
    {
        isBaking = true;
        fireEffect.SetActive(true);

        yield return new WaitForSeconds(bakingTime);

        fireEffect.SetActive(false);

        foreach (Transform doughSlot in doughSlots)
        {
            Instantiate(bakedBreadPrefab, doughSlot.position, Quaternion.identity);
        }

        // Platzhalter zurücksetzen
        foreach (GameObject placeholder in firewoodPlaceholders) placeholder.SetActive(false);
        foreach (GameObject placeholder in doughPlaceholders) placeholder.SetActive(false);

        firewoodCount = 0;
        doughCount = 0;
        isBaking = false;
    }
}
