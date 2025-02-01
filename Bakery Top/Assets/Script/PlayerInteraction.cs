using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 4f;
    public Transform playerCamera;
    public Animator playerAnimator;
    // Update is called once per frame
    void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        // Raycast aus der Kamera, um zu prüfen, ob ein Item im Blickfeld ist
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    string playerAnim = interactable.GetPlayerAnimation();
                    if (!string.IsNullOrEmpty(playerAnim))
                    {
                        playerAnimator.SetTrigger(playerAnim);
                    }
                    interactable.Interact();
                }
                Debug.Log(hit.transform.name);
                Debug.Log(interactable.GetInteractText());
            }
        }
    }
}
