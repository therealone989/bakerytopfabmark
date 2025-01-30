using UnityEngine;

public class Baum : MonoBehaviour, IInteractable
{

    [Header("Settings")]
    public GameObject woodPrefab;
    public Transform spawnPoint;
    public float spawnForce = 5f;


    public string GetInteractText()
    {
        return "Schlage den Baum mit der axt! (Linksclick)";
    }

    public void Interact()
    {
        SpawnWood();
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
}
