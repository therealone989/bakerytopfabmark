using Unity.Cinemachine;
using UnityEngine;

public class ShowItemHighlight : MonoBehaviour
{

    [Header("Settings")]
    public float highlightRange = 5f; // Reichweite des Highlightens
    public Transform playerCamera; // Die Kamera des Spielers

    private MeshRenderer highlightedObjectRenderer; // Das aktuell gehighlightete Objekt

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
            MeshRenderer hitRenderer = hit.collider.GetComponent<MeshRenderer>();

            // Prüft, ob das Objekt den "Grabbable"-Tag hat
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

        // Kein gültiges Objekt getroffen, Highlight entfernen
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
                outlineMaterial.SetFloat("_OutlineEnabled", 0f); // Aktiviert das Highlight
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
                outlineMaterial.SetFloat("_OutlineEnabled", 1f); // Deaktiviert das Highlight
            }
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
