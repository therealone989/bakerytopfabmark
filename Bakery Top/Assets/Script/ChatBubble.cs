using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    public SpriteRenderer backgroundSpriteRenderer;
    public TMP_Text interactText;
    private Transform playerCamera; // Referenz zur Kamera
    private void Start()
    {
        playerCamera = Camera.main.transform; // Kamera automatisch holen
    }

    private void Update()
    {
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera); // ChatBubble immer zur Kamera drehen
            transform.Rotate(0, 180, 0); // Optional: Dreht die Bubble um, falls sie falsch herum ist
        }
    }

    public void Setup(string text)
    {
        interactText.SetText(text);
        interactText.ForceMeshUpdate();

        // Holen der gerenderten Textgröße
        Vector2 textSize = interactText.GetRenderedValues(false);

        // Ein kleinerer Puffer für die Anpassung der Hintergrundgröße
        Vector2 padding = new Vector2(8f, 0.1f);

        // Skalierung des RectTransforms anpassen
        Debug.Log(textSize);

        backgroundSpriteRenderer.size = textSize + padding;
        // Hier wird die Skalierung des RectTransform verändert
        RectTransform rectTransform = backgroundSpriteRenderer.GetComponent<RectTransform>();
    }




    public static ChatBubble Create(Transform parent, Vector3 localPosition, string message)
    {
        GameObject chatBubblePrefab = Resources.Load<GameObject>("Prefabs/ChatBubble3D");

        if (chatBubblePrefab == null)
        {
            Debug.LogError("ChatBubble Prefab konnte nicht geladen werden!");
            return null;
        }

        // Erstelle die ChatBubble
        GameObject chatBubbleObject = Instantiate(chatBubblePrefab, parent);

        // **Fix: Skalierung nicht vom Parent übernehmen**
        chatBubbleObject.transform.SetParent(null, true);

        // Setze die Position und Größe richtig
        chatBubbleObject.transform.position = localPosition + new Vector3(0, 0, 0);
        chatBubbleObject.transform.localScale = Vector3.one; // Skaliere auf 1,1,1

        ChatBubble chatBubble = chatBubbleObject.GetComponent<ChatBubble>();
        chatBubble.Setup(message);

        return chatBubble;
    }

}
