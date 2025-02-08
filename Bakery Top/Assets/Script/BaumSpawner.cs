using System.Collections;
using UnityEngine;

public class BaumSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] baumPrefabs; // Array mit verschiedenen Baum-Prefabs
    public Transform[] spawnPoints; // Die festgelegten Positionen für Bäume
    public float respawnTime = 60f; // Zeit, bis ein Baum wieder erscheint

    private void Start()
    {
        StartCoroutine(RespawnRoutine()); // Startet die Überprüfung in einer Endlosschleife
    }

    private IEnumerator RespawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime); // Warte die eingestellte Zeit

            foreach (Transform spawnPoint in spawnPoints)
            {
                if (!BaumExistiertAnPosition(spawnPoint.position))
                {
                    SpawnRandomBaum(spawnPoint);
                }
            }
        }
    }

    private bool BaumExistiertAnPosition(Vector3 position)
    {
        // Überprüft, ob sich bereits ein Baum in der Nähe des Spawn-Punktes befindet
        Collider[] hitColliders = Physics.OverlapSphere(position, 1f); // 1f = kleiner Radius um den Punkt
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Baum")) // Falls ein Objekt mit dem Tag "Baum" existiert
            {
                return true;
            }
        }
        return false; // Falls kein Baum gefunden wurde
    }

    private void SpawnRandomBaum(Transform spawnPoint)
    {
        if (baumPrefabs.Length == 0) return; // Falls keine Prefabs vorhanden sind, tue nichts

        int randomIndex = Random.Range(0, baumPrefabs.Length); // Zufälliger Index aus dem Array
        GameObject neuerBaum = Instantiate(baumPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        neuerBaum.tag = "Baum"; // Setzt den Tag für die Existenzprüfung
    }
}
