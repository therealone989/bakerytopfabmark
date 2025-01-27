using UnityEngine;
using Unity.Cinemachine; // Import für Cinemachine

public class Grabitem : MonoBehaviour
{
    [Header("Settings")]
    public float grabRange = 5f;
    public float holdDistance = 2f;
    public float moveSpeed = 10f;
    public float throwForce = 500f;
    public float minHoldDistance = 1f;
    public float maxHoldDistance = 6f;
    public float snapThreshold = 0.5f;
    public float rotationSpeed = 50f;

    [Header("References")]
    public Transform playerCamera;
    public CinemachineInputAxisController cinemachineInputProvider;

    private Rigidbody grabbedObject;
    private MeshRenderer grabbedObjectRenderer;
    private bool isRotating = false;

    void Update()
    {
        HandleItemPickup(); // Hinzugefügt


        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }

        if (grabbedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            ThrowObject();
        }

        if (grabbedObject != null)
        {
            MoveGrabbedObject();
            AdjustHoldDistance();
            HandleRotation();
        }
    }

    private void HandleItemPickup()
    {
        // Raycast aus der Kamera, um zu prüfen, ob ein Item im Blickfeld ist
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            // Prüfen, ob das angeschautes Objekt ein "Item"-Skript hat
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                // Wenn "E" gedrückt wird, rufe PickUp auf
                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.PickUp(); // Methode im Item-Skript
                }
            }
        }
    }


    private void TryGrabObject()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null && hit.collider.CompareTag("Grabbable"))
            {
                grabbedObject = rb;
                grabbedObjectRenderer = hit.collider.GetComponent<MeshRenderer>();
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
            grabbedObjectRenderer = null;
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.interpolation = RigidbodyInterpolation.None;
            grabbedObject.useGravity = true;
            grabbedObject.linearDamping = 1f;
            grabbedObject = null;
            grabbedObjectRenderer = null;
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
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotating = true;

            // Setzt die Rotation und Winkelgeschwindigkeit des Objekts zurück
            grabbedObject.transform.rotation = Quaternion.identity;
            grabbedObject.angularVelocity = Vector3.zero;

            // Deaktiviert die Kamerasteuerung
            cinemachineInputProvider.enabled = false;
        }

        if (Input.GetKey(KeyCode.R) && isRotating)
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            grabbedObject.transform.Rotate(playerCamera.up, -rotationX, Space.World);
            grabbedObject.transform.Rotate(playerCamera.right, rotationY, Space.World);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            isRotating = false;

            // Aktiviert die Kamerasteuerung wieder
            cinemachineInputProvider.enabled = true;
        }
    }
}
