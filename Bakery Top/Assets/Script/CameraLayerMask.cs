using UnityEngine;

public class CameraLayerMask : MonoBehaviour
{
    [Header("Layer zum Ausblenden")]
    [SerializeField] private string layerToHide;

    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(cam == null)
        {
            Debug.LogError("Kein Camera-Component gefunden!");
            return;
        }
        int layerIndex = LayerMask.NameToLayer(layerToHide);

        if (layerIndex == -1)
        {
            Debug.LogError("Layer \"" + layerToHide + "\" existiert nicht! Stelle sicher, dass der Name korrekt ist.");
            return;
        }

        cam.cullingMask &= ~(1 << layerIndex);
    }
}
