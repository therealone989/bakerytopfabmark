using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [Header("Spawner Einstellungen")]
    public GameObject[] pedestrianPrefabs;    // Array von Passanten-Prefabs
    public float minSpawnInterval = 2f;        // Kürzester Intervall
    public float maxSpawnInterval = 5f;        // Längster Intervall
    public Transform spawnPoint;               // Startposition für den Passanten

    [Header("Wegpunkt Einstellungen")]
    public Transform[] waypoints;              // Array der Wegpunkte, die der Passant ablaufen soll

    private float timer;
    private float randomSpawnInterval;

    private void Start()
    {
        // Setze den zufälligen Intervall zu Beginn für jeden Spawner
        randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Wenn der Timer den zufällig bestimmten Intervall überschreitet
        if (timer >= randomSpawnInterval)
        {
            SpawnPedestrian();
            timer = 0f;

            // Setze den Timer mit einem neuen zufälligen Intervall zurück
            randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnPedestrian()
    {
        // Wähle zufällig ein Prefab aus dem Array
        int randomIndex = Random.Range(0, pedestrianPrefabs.Length);
        GameObject selectedPrefab = pedestrianPrefabs[randomIndex];

        // Erstelle den Passanten an der Spawn-Position
        GameObject pedestrian = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

        // Hole das PedestrianMovement-Skript und übergebe die Waypoints
        PedestrianMovement movement = pedestrian.GetComponent<PedestrianMovement>();
        if (movement != null)
        {
            movement.SetWaypoints(waypoints);
        }
        else
        {
            Debug.LogWarning("Das Passanten-Prefab hat kein PedestrianMovement-Skript angehängt.");
        }
    }
}
