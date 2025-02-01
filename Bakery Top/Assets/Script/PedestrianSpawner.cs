using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject pedestrianPrefab;   // Das Passanten-Prefab
    public float spawnInterval = 3f;      // Zeitintervall zwischen den Spawn-Vorgängen
    public Transform spawnPoint;          // Startposition für den Passanten

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPedestrian();
            timer = 0f;
        }
    }

    private void SpawnPedestrian()
    {
        // Erstelle einen neuen Passanten an der Spawn-Position
        Instantiate(pedestrianPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
