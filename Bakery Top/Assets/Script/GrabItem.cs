using UnityEngine;

public class Grabitem : MonoBehaviour
{
    [Header("Settings")]
    public float grabRange = 5f; // Reichweite des Greifens
    public float holdDistance = 2f; // Abstand des gehaltenen Objekts zur Kamera
    public float moveSpeed = 10f; // Wie schnell das Objekt bewegt wird
    public float throwForce = 500f; // Die Wurfkraft
    public float minHoldDistance = 1f; // Minimaler Halteabstand
    public float maxHoldDistance = 6f; // Maximaler Halteabstand
    public float snapThreshold = 0.5f; // Höhe für das "Snapping"
    public float rotationSpeed = 50f; // Rotationsgeschwindigkeit

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

        // Objekt werfen mit der Taste F
        if (grabbedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            ThrowObject();
        }

        // Bewegt das gehaltene Objekt
        if (grabbedObject != null)
        {
            MoveGrabbedObject();

            // Halteabstand mit Mausrad anpassen
            AdjustHoldDistance();

            // Objektrotation mit R anpassen
            RotateObject();
        }
    }

    private void HandleHighlight()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * grabRange, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            MeshRenderer hitRenderer = hit.collider.GetComponent<MeshRenderer>();

            if (hit.collider.CompareTag("Grabbable") && hitRenderer != null)
            {
                if (highlightedObjectRenderer != hitRenderer)
                {
                    DisableHighlight(highlightedObjectRenderer);
                    EnableHighlight(hitRenderer);
                    highlightedObjectRenderer = hitRenderer;
                }
                return;
            }
        }

        DisableHighlight(highlightedObjectRenderer);
        highlightedObjectRenderer = null;
    }

    private void EnableHighlight(MeshRenderer renderer)
    {
        if (renderer != null && renderer.materials.Length > 1)
        {
            Material outlineMaterial = FindMaterialByName(renderer, "Outline");
            if (outlineMaterial.HasProperty("_OutlineEnabled"))
            {
                outlineMaterial.SetFloat("_OutlineEnabled", 0f);
            }
        }
    }

    private void DisableHighlight(MeshRenderer renderer)
    {
        if (renderer != null && renderer.materials.Length > 1)
        {
            Material outlineMaterial = FindMaterialByName(renderer, "Outline");
            if (outlineMaterial.HasProperty("_OutlineEnabled"))
            {
                outlineMaterial.SetFloat("_OutlineEnabled", 1f);
            }
        }
    }

    private void TryGrabObject()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            MeshRenderer mr = hit.collider.GetComponent<MeshRenderer>();

            if (rb != null && hit.collider.CompareTag("Grabbable"))
            {
                grabbedObject = rb;
                grabbedObjectRenderer = mr;

                EnableHighlight(grabbedObjectRenderer);

                grabbedObject.useGravity = false;
                grabbedObject.linearDamping = 10f;
                grabbedObject.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }

    private void ThrowObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.linearDamping = 1f;

            grabbedObject.AddForce(playerCamera.forward * throwForce);

            grabbedObject = null;

            if (highlightedObjectRenderer != grabbedObjectRenderer)
            {
                DisableHighlight(grabbedObjectRenderer);
            }

            grabbedObjectRenderer = null;

            Debug.Log("Object thrown!");
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.interpolation = RigidbodyInterpolation.None;
            grabbedObject.useGravity = true;
            grabbedObject.linearDamping = 1f;

            // Snap auf Oberfläche
            SnapToSurface(grabbedObject);

            grabbedObject = null;

            if (highlightedObjectRenderer != grabbedObjectRenderer)
            {
                DisableHighlight(grabbedObjectRenderer);
            }

            grabbedObjectRenderer = null;
        }
    }

    private void SnapToSurface(Rigidbody obj)
    {
        if (Physics.Raycast(obj.position, Vector3.down, out RaycastHit hit, snapThreshold))
        {
            obj.position = hit.point;
        }
    }

    private void MoveGrabbedObject()
    {
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * holdDistance;
        Vector3 moveDirection = (targetPosition - grabbedObject.position);
        grabbedObject.linearVelocity = moveDirection * moveSpeed;
    }

    private void AdjustHoldDistance()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            holdDistance = Mathf.Clamp(holdDistance + scroll, minHoldDistance, maxHoldDistance);
            Debug.Log("Hold Distance: " + holdDistance);
        }
    }

    private void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            grabbedObject.transform.Rotate(playerCamera.up, -rotationX, Space.World);
            grabbedObject.transform.Rotate(playerCamera.right, rotationY, Space.World);
        }
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
