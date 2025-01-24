using UnityEngine;

public class Grabitem : MonoBehaviour
{
    [Header("Settings")]
    public float grabRange = 5f; // Reichweite des Greifens
    public float holdDistance = 2f; // Abstand des gehaltenen Objekts zur Kamera
    public float moveSpeed = 10f; // Wie schnell das Objekt bewegt wird

    [Header("References")]
    public Transform playerCamera; // Die Kamera des Spielers

    private Rigidbody grabbedObject; // Das aktuell gehaltene Objekt

    private MeshRenderer objectOutline;

    void Update()
    {
        // Prüft, ob der Spieler mit der linken Maustaste klickt
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
            {
                TryGrabObject(); // Versucht, ein Objekt zu greifen
                if (grabbedObject != null) // Prüft, ob tatsächlich ein Objekt gegriffen wurde
                {
                    Debug.Log("Grabbed: " + grabbedObject.transform.name);
                }
                else
                {
                    Debug.Log("Kein Objekt gefunden!");
                }
            }
            else
            {
                ReleaseObject(); // Lässt das Objekt los
            }
        }

        // Bewegt das gehaltene Objekt
        if (grabbedObject != null)
        {
            MoveGrabbedObject();
        }
    }

    private void TryGrabObject()
    {
        // Raycast von der Kamera in Blickrichtung
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        // Zeichnet den Ray im Scene View
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            // Prüft, ob das getroffene Objekt einen Rigidbody hat
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            MeshRenderer mr = hit.collider.GetComponent<MeshRenderer>();
            if (rb != null)
            {
                grabbedObject = rb;
                objectOutline = mr;

                objectOutline.materials[1].SetFloat("_Mode", 3);
                grabbedObject.useGravity = false; // Deaktiviert die Schwerkraft
                grabbedObject.linearDamping = 10f; // Erhöht den Widerstand, um unnatürliche Bewegungen zu vermeiden
                grabbedObject.interpolation = RigidbodyInterpolation.Interpolate;

            }
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.interpolation = RigidbodyInterpolation.None;
            grabbedObject.useGravity = true; // Aktiviert die Schwerkraft
            grabbedObject.linearDamping = 1f; // Setzt den Standard-Widerstand zur�ck
            grabbedObject = null;
            

        }
    }

    private void MoveGrabbedObject()
    {
        // Zielposition vor der Kamera
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * holdDistance;
        Vector3 moveDirection = (targetPosition - grabbedObject.position);
        grabbedObject.linearVelocity = moveDirection * moveSpeed; // Bewegt das Objekt zur Zielposition
    }
}
    
