using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Baum : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public GameObject woodPrefab;
    public Transform spawnPoint;
    public float spawnForce = 5f;
    private bool canChop = true;
    public float chopCooldown = 1.5f;

    private int chopCount = 0; // Zähler für die Anzahl der Schläge
    public int maxChops = 5;   // Maximale Anzahl an Schlägen, bevor der Baum zerstört wird

    public string GetInteractText()
    {
        return canChop ? "Schlage den Baum mit der Axt! (Linksklick)" : "Warte noch einen Moment!";
    }

    public void Interact()
    {
        if (!canChop) return;  // Verhindert mehrfaches Spammen

        SpawnWood();
        chopCount++;  // Erhöhe den Schlag-Zähler

        if (chopCount >= maxChops)
        {
            DestroyTree(); // Baum zerstören, wenn das Limit erreicht ist
        }
        else
        {
            StartCoroutine(ChopCooldown());
        }
    }

    public string GetPlayerAnimation()
    {
        return canChop ? "CutWood" : "";  // Falls canChop false ist, gibt es keinen Animation-String zurück
    }

    private void SpawnWood()
    {
        if (woodPrefab != null && spawnPoint != null)
        {
            // Holz-Objekt instanziieren
            GameObject wood = Instantiate(woodPrefab, spawnPoint.position, Random.rotation);

            // Rigidbody hinzufügen und Wurfkraft anwenden
            Rigidbody rb = wood.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Zufällige Wurfrichtung
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1f), // Leicht nach oben werfen
                    Random.Range(-1f, 1f)
                ).normalized;

                rb.AddForce(randomDirection * spawnForce, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("woodPrefab oder spawnPoint fehlt im Baum-Skript!");
        }
    }

    private IEnumerator ChopCooldown()
    {
        canChop = false;
        yield return new WaitForSeconds(chopCooldown);
        canChop = true;
    }

    private void DestroyTree()
    {
        Debug.Log("Der Baum wurde gefällt!"); // Debug-Meldung zur Überprüfung
        Destroy(gameObject); // Löscht das GameObject, an dem das Skript hängt
    }
}
