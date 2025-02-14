using System;
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
    public int maxChops = 5;   // Maximale Anzahl an Schlägen

    [Header("Baum Wachstum")]
    public float shrinkTime = 1f;    // Dauer, bis der Baum geschrumpft ist
    public float regrowDelay = 5f;   // Wie lange bleibt der Baum klein?
    public float growTime = 3f;      // Dauer des Wachstums

    private Vector3 originalScale;   // Speichert die normale Größe

    private void Start()
    {
        originalScale = transform.localScale; // Speichert die normale Größe
    }

    public string GetInteractText()
    {
        return canChop ? "Schlage den Baum mit der Axt! (Linksklick)" : "Warte einen Moment!";
    }

    public void Interact()
    {
        if (!canChop) return;  // Verhindert mehrfaches Spammen

        SpawnWood();
        chopCount++;

        if (chopCount >= maxChops)
        {
            StartCoroutine(ShrinkTree()); // Baum wird gefällt und startet den Zyklus
        }
        else
        {
            StartCoroutine(ChopCooldown());
        }
    }

    public string GetPlayerAnimation()
    {
        return canChop ? "CutWood" : "";
    }

    private void SpawnWood()
    {
        if (woodPrefab != null && spawnPoint != null)
        {
            GameObject wood = Instantiate(woodPrefab, spawnPoint.position, UnityEngine.Random.rotation);
            Rigidbody rb = wood.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 randomDirection = new Vector3(
                   UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(0.5f, 1f),
                    UnityEngine.Random.Range(-1f, 1f)
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

    private IEnumerator ShrinkTree()
    {
        canChop = false; // Baum kann nicht mehr gehackt werden
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero; // Baum verschwindet

        while (elapsedTime < shrinkTime)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero; // Falls Lerp nicht exakt trifft
        gameObject.SetActive(false); // Baum ist unsichtbar und nicht interaktiv

        //yield return new WaitForSeconds(regrowDelay); // Wartezeit, bevor er wächst

        Invoke(nameof(ReactivateAndGrow), regrowDelay);

        
    }

    private void ReactivateAndGrow()
    {
        gameObject.SetActive(true); // Baum ist unsichtbar und nicht interaktiv
        StartCoroutine(GrowTree());
    }

    private IEnumerator GrowTree()
    {
        Debug.Log("GGROOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOW");
        gameObject.SetActive(true); // Baum wird wieder sichtbar
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = originalScale; // Baum wächst auf Originalgröße

        while (elapsedTime < growTime)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / growTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; // Finale Größe setzen
        chopCount = 0; // Baum ist wieder hackbar
        canChop = true;
    }
}
