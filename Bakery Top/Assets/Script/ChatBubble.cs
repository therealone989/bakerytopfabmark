using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class ChatBubble : MonoBehaviour
{
    public RectTransform backgroundSpriteRenderer;
    public TMP_Text interactText;

    private void Start()
    {
        Setup("Wassup my man hehehe");
    }

    private void Setup(string text)
    {
        interactText.SetText(text);
        interactText.ForceMeshUpdate();
        Vector2 textSize = interactText.GetRenderedValues(false);
        Vector2 padding = new Vector2(30f, 50f);
        backgroundSpriteRenderer.sizeDelta = textSize + padding;
    }

    public static ChatBubble Create(Transform parent, Vector3 localPosition, string message)
    {
        // Instantiate the prefab
        GameObject chatBubblePrefab = Resources.Load<GameObject>("Prefabs/ChatBubble3D"); ;
        GameObject chatBubbleObject = Instantiate(chatBubblePrefab, parent);

        chatBubbleObject.transform.localPosition = localPosition;

        // Setup the ChatBubble
        ChatBubble chatBubble = chatBubbleObject.GetComponent<ChatBubble>();
        chatBubble.Setup(message);

        // Destroy after 4 seconds
        Destroy(chatBubbleObject, 2f);

        return chatBubble;
    }
}
