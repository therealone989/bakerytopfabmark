using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    [Header("UI and Camera Settings")]
    public Canvas mouseCanvas;
    [SerializeField] private MoveForce playerMovement;
    [SerializeField] private GameObject cineCam;
    [SerializeField] private Grabitem grabItemScript;

    [SerializeField] public GameObject statsMenu;
    [SerializeField] public Button nextMenuButton;

    [SerializeField] public Image itemDescriptionImage;
    [SerializeField] public TMP_Text ItemDescriptionNameText;
    [SerializeField] public TMP_Text ItemDescriptionText;

    [SerializeField] private Sprite emptySprite;

    public bool isInInventory = false;


    // Update is called once per frame
    void Update()
    {

        if (grabItemScript != null && grabItemScript.isGrabbing == true)
        {
            Debug.Log("IS HALTING");
            return;
        }

        if(Input.GetButtonDown("Inventory") && menuActivated)
        {
            ToggleInventory(false);
            DeselectAllSlots();
            isInInventory = true;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            // Time.timeScale = 0;  // OPTIONAL -- STOPPT ZEIT BEI AKTIVEN MEN� - Physics Stoppen auch, Animations k�nnen Fehler geben
            ToggleInventory(true);
            DeselectAllSlots();
            isInInventory = false;
        }
    }

    public int AddItem(string itemName, int quantity, Sprite[] itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false && itemSlot[i].itemName == itemName|| itemSlot[i].quantity == 0)
            {
                // Rekursiv falls das inventar voll ist, soll es dann zum n�chsten slot gehen weil es ja voll ist und der name ist gleich
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);

                // WENN ES RESTE GIBT DANN
                if(leftOverItems > 0)
                {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }

        itemDescriptionImage.sprite = emptySprite;
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
    }

    public void ToggleInventory(bool isActive)
    {

        InventoryMenu.SetActive(isActive);
        statsMenu.SetActive(false);
        menuActivated = isActive;
        nextMenuButton.gameObject.SetActive(isActive);
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;

        StopMovementInInventory(isActive);

        if (!isActive)
        {
            
            DeselectAllSlots();
        }
    }

    private void StopMovementInInventory(bool isActive)
    {
        if (mouseCanvas != null)
        {
            mouseCanvas.enabled = !isActive;
        }

        if (playerMovement != null)
        {
            //playerMovement.enabled = !isActive;

            // **Physik stoppen, wenn das Inventar aktiv ist**
            Rigidbody rb = playerMovement.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (isActive)
                {
                    playerMovement.speed = 0f;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true; // **Blockiert alle weiteren Bewegungen**
                }
                else
                {
                    rb.isKinematic = false; // **Physik wieder aktivieren**
                }
            }
        }

        if (cineCam != null)
        {
            cineCam.SetActive(!isActive);
        }

        if (grabItemScript != null)
        {
            grabItemScript.enabled = !isActive;
        }
    }


    public void ToggleMenus()
    {
        if(InventoryMenu.gameObject.activeSelf)
        {
            InventoryMenu.gameObject.SetActive(false);
            statsMenu.gameObject.SetActive(true);
        } else if (statsMenu.gameObject.activeSelf)
        {
            InventoryMenu.gameObject.SetActive(true);
            statsMenu.gameObject.SetActive(false);
        }
    }

}
