using UnityEngine;
using System.Collections;

public class Oven : MonoBehaviour
{
    [Header ("Einstellungen")]
    public Transform spawnPoint;
    public string requiredItemName = "Dough";
    public string requiredFuelName = "Firewood";
    public GameObject fireEffect;
    public float bakingTime = 5f;
    public float fuelBurnTime = 10f;

    private Item currentItem;
    private bool isBaking = false;
    private bool isHeated = false; // Ob der Ofen heiß ist
    private Animator animator;
    private Coroutine burnCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            if (item.itemName == requiredFuelName)
            {
                AddFuel(item);
            }
            else if (item.itemName == requiredItemName && isHeated)
            {
                currentItem = item;
                Debug.Log("Dough auf den Ofen gelegt: " + item.itemName);

            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item == currentItem)
        {
            currentItem = null;
            Debug.Log("Dough wurde vom Ofen entfernt.");
        }
    }

    private void AddFuel(Item fuelItem)
    {
        if (!isHeated) 
        {
            Destroy(fuelItem.gameObject);
            isHeated = true;
            Debug.Log("Feuerholz hinzugefügt. Der Ofen wird erhitzt!");

           
            if (fireEffect != null)
                fireEffect.SetActive(true);

            burnCoroutine = StartCoroutine(BurnFuel());
        }
        else
        {
            Debug.Log("Der Ofen ist bereits heiß.");
        }
    }

    private IEnumerator BurnFuel()
    {
        yield return new WaitForSeconds(fuelBurnTime);
        isHeated = false;
        Debug.Log("Das Feuer ist erloschen.");

        if (fireEffect != null)
            fireEffect.SetActive(false);
    }

    private IEnumerator BakeBread()
    {
        isBaking = true;
        animator.SetTrigger("StartBaking"); 

        Debug.Log("Backvorgang gestartet...");
        yield return new WaitForSeconds(bakingTime);

        Debug.Log("Backvorgang beendet!");

        // Dough zerstören
        if (currentItem != null)
        {
            Destroy(currentItem.gameObject);
            currentItem = null;
        }

        // Brot spawnen
        GameObject breadPrefab = Resources.Load<GameObject>("Prefabs/Bread");
        if (breadPrefab != null)
        {
            Instantiate(breadPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Bread Prefab nicht gefunden in Resources/Prefabs!");
        }

        animator.SetTrigger("EndBaking"); 
        isBaking = false;
    }

    public void StartBaking()
    {
        if (!isBaking && currentItem != null && isHeated)
        {
            animator.SetTrigger("OpenOven");
            StartCoroutine(BakeBread());
        }
        else if (!isHeated)
        {
            Debug.Log("Der Ofen ist nicht heiß genug zum Backen!");
        }
        else
        {
            Debug.Log("Kein Teig im Ofen!");
        }
    }
}
