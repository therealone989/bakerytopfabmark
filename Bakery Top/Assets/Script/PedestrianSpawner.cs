using UnityEngine;
using UnityEngine.AI;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject pedestrianPrefab;
    public Transform spawnPoint;
    public Transform bakeryEntrance;  // Ziel: Eingang der Bäckerei
    public float spawnDelay = 2f;     // Verzögerung bis der Passant spawnt

    private bool hasSpawned = false;  // Damit nur einer spawnt

    private void Start()
    {
        if (bakeryEntrance == null)
        {
            Debug.LogError("❌ BakeryEntrance ist nicht zugewiesen! Bitte im Inspector setzen.");
        }
        else
        {
            Debug.Log("✅ BakeryEntrance ist korrekt gesetzt: " + bakeryEntrance.position);
        }

        Invoke("SpawnPedestrian", spawnDelay);  // Warten und dann Passanten spawnen
    }

    private void SpawnPedestrian()
    {
        if (hasSpawned) return;  // Falls schon einer gespawnt wurde, nichts tun
        hasSpawned = true;

        GameObject pedestrian = Instantiate(pedestrianPrefab, spawnPoint.position, Quaternion.identity);
        NavMeshAgent agent = pedestrian.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            Debug.Log("🚶 Pedestrian gespawnt und NavMeshAgent gefunden!");
            bool success = agent.SetDestination(bakeryEntrance.position);

            if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Debug.LogError("❌ Der NavMeshAgent konnte keinen Pfad berechnen! Liegt das Ziel auf dem NavMesh?");
            }

            if (success)
            {
                Debug.Log("✅ Ziel gesetzt: " + bakeryEntrance.position);
                StartCoroutine(WaitUntilArrival(agent)); // Überwachen, wann er ankommt
            }
            else
            {
                Debug.LogError("❌ SetDestination hat nicht funktioniert!");
            }
        }
        else
        {
            Debug.LogError("❌ Das Prefab hat keinen NavMeshAgent!");
        }
    }

    private System.Collections.IEnumerator WaitUntilArrival(NavMeshAgent agent)
    {
        while (!agent.pathPending && agent.remainingDistance > 0.5f)
        {
            Debug.Log("🚶‍♂️ Läuft... Distanz zum Ziel: " + agent.remainingDistance);
            yield return null;  // Warten, bis er das Ziel erreicht
        }

        agent.isStopped = true;  // Bewegung stoppen
        Debug.Log("🏁 Pedestrian hat das Ziel erreicht!");
    }
}
