using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 4f;
    public Transform playerCamera;
    public Animator playerAnimator;

    private ChatBubble currentBubble;
    private IInteractable lastInteractable;

    void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Erstelle die ChatBubble nur, wenn das Ziel sich geändert hat!
                if (lastInteractable != interactable)
                {
                    //ShowChatBubble(interactable, hit.transform);
                    lastInteractable = interactable; // Merke das aktuelle Interactable
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    string playerAnim = interactable.GetPlayerAnimation();
                    if (!string.IsNullOrEmpty(playerAnim))
                    {
                        playerAnimator.SetTrigger(playerAnim);
                    }
                    interactable.Interact();
                    //HideChatBubble(); // Blende die ChatBubble nach der Interaktion aus
                }
            }
        }
        else
        {
            // Wenn kein Objekt mehr im Fokus ist, entferne die ChatBubble
            //HideChatBubble();
        }
    }

    //private void ShowChatBubble(IInteractable interactable, Transform itemTransform)
    //{
    //    if (currentBubble != null)
    //    {
    //        Destroy(currentBubble.gameObject); // Entferne alte Bubble
    //    }

    //    string message = interactable.GetInteractText();

    //    // Spawne die Bubble relativ zum Item!
    //    currentBubble = ChatBubble.Create(itemTransform, itemTransform.position + new Vector3(0, 1f, 0), message);
    //}

    //private void HideChatBubble()
    //{
    //    if (currentBubble != null)
    //    {
    //        Destroy(currentBubble.gameObject);
    //        currentBubble = null;
    //        lastInteractable = null;
    //    }
    //}


}
