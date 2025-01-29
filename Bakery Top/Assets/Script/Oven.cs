using UnityEngine;
using System.Collections;

public class Oven : MonoBehaviour
{
    public Transform spawnPoint;  // Wo das Brot erscheinen soll
    public string requiredItemName = "Dough";  // Der Name des Items, das gebacken werden kann
    public GameObject fireEffect;  // Optional: Feuer-Effekt beim Backen

    private Item currentItem;
    private bool isBaking = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null && item.itemName == requiredItemName) // Prüfen, ob es Dough ist
        {
            currentItem = item;
            Debug.Log($"Dough auf den Ofen gelegt: {item.itemName}");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item == currentItem) // Wenn das aktuelle Item entfernt wird
        {
            currentItem = null;
            Debug.Log("Dough wurde vom Ofen entfernt.");
        }
    }

    private IEnumerator BakeBread()
    {
        isBaking = true;

        // Feuer-Effekt aktivieren (falls vorhanden)
        if (fireEffect != null)
        {
            fireEffect.SetActive(true);
        }

        Debug.Log("Backvorgang gestartet...");

        yield return new WaitForSeconds(5);  // 5 Sekunden Backzeit

        Debug.Log("Backvorgang beendet!");

        // Dough zerstören
        if (currentItem != null)
        {
            Destroy(currentItem.gameObject);
            currentItem = null;
        }

        // Brot spawnen
        GameObject breadPrefab = Resources.Load<GameObject>("Prefabs/Pan Bimbo");
        if (breadPrefab != null)
        {
            Instantiate(breadPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Bread Prefab nicht gefunden in Resources/Prefabs!");
        }

        // Feuer-Effekt deaktivieren (falls vorhanden)
        if (fireEffect != null)
        {
            fireEffect.SetActive(false);
        }

        isBaking = false;
        animator.SetTrigger("ButtonBack");
    }

    public void StartBaking()
    {
        if (!isBaking && currentItem != null)
        {
            animator.SetTrigger("ButtonPressed");
            StartCoroutine(BakeBread());

        }
    }
}
