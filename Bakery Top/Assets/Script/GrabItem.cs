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
    private MeshRenderer highlightedObjectRenderer; // Das aktuell gehighlightete Objekt
    private MeshRenderer grabbedObjectRenderer; // Renderer des gehaltenen Objekts

    void Update()
    {
        // Highlight-Logik
        HandleHighlight();

        // Greifen und Loslassen mit Mausklick
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
            {
                TryGrabObject(); // Versucht, ein Objekt zu greifen
                if (grabbedObject != null)
                {
                    Debug.Log("Grabbed: " + grabbedObject.transform.name);
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

    private void HandleHighlight()
    {
        // Raycast von der Kamera in Blickrichtung
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        // Zeichnet den Ray im Scene View
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            MeshRenderer hitRenderer = hit.collider.GetComponent<MeshRenderer>();

            // Prüft, ob das Objekt den "Grabbable"-Tag hat
            if (hit.collider.CompareTag("Grabbable") && hitRenderer != null)
            {
                // Falls ein neues Objekt getroffen wurde
                if (highlightedObjectRenderer != hitRenderer)
                {
                    // Deaktiviere das alte Highlight
                    DisableHighlight(highlightedObjectRenderer);

                    // Aktiviere das Highlight für das neue Objekt
                    EnableHighlight(hitRenderer);
                    highlightedObjectRenderer = hitRenderer;
                }

                return;
            }
        }

        // Kein gültiges Objekt getroffen, Highlight entfernen
        DisableHighlight(highlightedObjectRenderer);
        highlightedObjectRenderer = null;
    }

    private void EnableHighlight(MeshRenderer renderer)
    {
        if (renderer != null && renderer.materials.Length > 1)
        {
            Material outlineMaterial = FindMaterialByName(renderer, "Outline"); // Zweites Material
            if (outlineMaterial.HasProperty("_OutlineEnabled"))
            {
                outlineMaterial.SetFloat("_OutlineEnabled", 0f); // Aktiviert das Highlight
            }
        }
    }

    private void DisableHighlight(MeshRenderer renderer)
    {
        if (renderer != null && renderer.materials.Length > 1)
        {
            Material outlineMaterial = FindMaterialByName(renderer, "Outline"); // Zweites Material
            if (outlineMaterial.HasProperty("_OutlineEnabled"))
            {
                outlineMaterial.SetFloat("_OutlineEnabled", 1f); // Deaktiviert das Highlight
            }
        }
    }

    private void TryGrabObject()
    {
        // Raycast von der Kamera in Blickrichtung
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            MeshRenderer mr = hit.collider.GetComponent<MeshRenderer>();

            if (rb != null && hit.collider.CompareTag("Grabbable"))
            {
                grabbedObject = rb;
                grabbedObjectRenderer = mr;

                // Outline bleibt aktiv, wenn gegriffen
                EnableHighlight(grabbedObjectRenderer);

                grabbedObject.useGravity = false; // Deaktiviert die Schwerkraft
                grabbedObject.linearDamping = 10f;
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
            grabbedObject.linearDamping = 1f;
            grabbedObject = null;

            // Highlight bleibt aktiv, wenn das Objekt noch angeschaut wird
            if (highlightedObjectRenderer != grabbedObjectRenderer)
            {
                DisableHighlight(grabbedObjectRenderer);
            }

            grabbedObjectRenderer = null;
        }
    }

    private void MoveGrabbedObject()
    {
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * holdDistance;
        Vector3 moveDirection = (targetPosition - grabbedObject.position);
        grabbedObject.linearVelocity = moveDirection * moveSpeed;
    }

    private Material FindMaterialByName(MeshRenderer renderer, string materialName)
    {
        foreach (var material in renderer.materials)
        {
            if (material.name.Contains(materialName))
            {
                return material;
            }
        }
        return null;
    }
}
