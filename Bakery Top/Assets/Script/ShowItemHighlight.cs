using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemHighlight : MonoBehaviour
{

    [Header("Settings")]
    public float highlightRange = 4f; // Reichweite des Highlightens
    public Transform playerCamera; // Die Kamera des Spielers

    [SerializeField] Sprite normalCursor;
    [SerializeField] Sprite highlightCursor;
    [SerializeField] Image cursorImage;



    void Update()
    {
        HandleHighlight();
    }

    private void HandleHighlight()
    {
        // Raycast von der Kamera in Blickrichtung
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * highlightRange, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, highlightRange))
        {
            // Prüft, ob das Objekt den "Grabbable"-Tag hat
            if (hit.collider.CompareTag("Grabbable"))
            {
                cursorImage.sprite = highlightCursor;

                return;
            }
        }

        cursorImage.sprite = normalCursor;

    }


}
