using TMPro;
using UnityEngine;

public class FPSAnzeige : MonoBehaviour
{
    private float deltaTime = 0.0f;
    public TextMeshProUGUI fpsText;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.Ceil(fps)}"; // FPS aufrunden f�r bessere Lesbarkeit
    }
}
