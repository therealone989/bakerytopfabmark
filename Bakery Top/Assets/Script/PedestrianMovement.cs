using UnityEngine;

public class PedestrianMovement : MonoBehaviour
{
    private Transform[] waypoints;      // Wird vom Spawner gesetzt
    public float moveSpeed = 2f;        // Geschwindigkeit des Passanten
    public float reachDistance = 0.1f;  // Abstand, bei dem ein Wegpunkt als erreicht gilt

    private int currentWaypointIndex = 0;

    /// <summary>
    /// Setzt die Waypoints, die der Passant ablaufen soll.
    /// Wird vom Spawner beim Instanziieren aufgerufen.
    /// </summary>
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;

        if (waypoints != null && waypoints.Length > 0)
        {
            // Die Startposition des Passanten bleibt an der Spawn-Position, 
            // der Passant bewegt sich direkt zum ersten Wegpunkt.
        }
        else
        {
            Debug.LogError("Keine Waypoints übergeben!");
        }
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        // Zielwegpunkt abrufen
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Richtung berechnen und Passanten bewegen
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optional: Drehung in Blickrichtung des Ziels
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }

        // Prüfe, ob der aktuelle Wegpunkt erreicht wurde
        if (Vector3.Distance(transform.position, targetWaypoint.position) < reachDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Den Passanten zerstören, wenn der letzte Wegpunkt erreicht ist
                Destroy(gameObject);
            }
        }
    }
}
