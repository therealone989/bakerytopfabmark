using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Baum : MonoBehaviour, IInteractable
{

    [Header("Settings")]
    public GameObject woodPrefab;
    public Transform spawnPoint;
    public float spawnForce = 5f;
    private bool canChop = false;
    public float chopCooldown = 2f;


    public string GetInteractText()
    {
        return canChop ? "Schlage den Baum mit der axt! (Linksclick)" : "Warte noch ein Moment bis du schlagen kannst!";
    }

    public void Interact()
    {
        SpawnWood();
        StartCoroutine(ChopCooldown());
    }

    public string GetPlayerAnimation()
    {
        return "CutWood";
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
                // Zuf舁lige Wurfrichtung
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
}
