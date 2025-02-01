using UnityEngine;

public class PedestrianMovement : MonoBehaviour
{
    [Header("Wegpunkt Einstellungen")]
    public Transform[] waypoints;      // Array der Wegpunkte, die der Passant ablaufen soll
    public float moveSpeed = 2f;       // Geschwindigkeit des Passanten
    public float reachDistance = 0.1f; // Distanz, bei der der Wegpunkt als "erreicht" gilt

    private int currentWaypointIndex = 0;

    private void Start()
    {
        if (waypoints.Length > 0)
        {
            // Setze die Startposition des Passanten auf den ersten Wegpunkt (optional)
            transform.position = waypoints[0].position;
        }
        else
        {
            Debug.LogError("Keine Wegpunkte zugewiesen!");
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0)
            return;

        // Bewege dich in Richtung des aktuellen Wegpunkts
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optional: Passanten können sich auch in Blickrichtung drehen
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }

        // Prüfe, ob der Wegpunkt erreicht wurde
        if (Vector3.Distance(transform.position, targetWaypoint.position) < reachDistance)
        {
            // Wechsle zum nächsten Wegpunkt (oder bleibe stehen, falls alle erreicht wurden)
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Beispiel: Stoppe am letzten Wegpunkt
                currentWaypointIndex = waypoints.Length - 1;
                // Alternativ: Zerstöre den Passanten oder starte den Weg von vorn:
                // currentWaypointIndex = 0;
                // oder: Destroy(gameObject);
            }
        }
    }
}
