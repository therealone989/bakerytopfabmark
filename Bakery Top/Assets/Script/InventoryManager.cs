using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    [Header("UI and Camera Settings")]
    public Canvas mouseCanvas;
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private GameObject cineCam;
    [SerializeField] private Grabitem grabItemScript;

    [SerializeField] public GameObject statsMenu;
    [SerializeField] public Button nextMenuButton;




    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory") && menuActivated)
        {
            ToggleInventory(false);
            DeselectAllSlots();
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            // Time.timeScale = 0;  // OPTIONAL -- STOPPT ZEIT BEI AKTIVEN MENÜ - Physics Stoppen auch, Animations können Fehler geben
            ToggleInventory(true);
            DeselectAllSlots();
        }
    }

    public int AddItem(string itemName, int quantity, Sprite[] itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false && itemSlot[i].itemName == itemName|| itemSlot[i].quantity == 0)
            {
                // Rekursiv falls das inventar voll ist, soll es dann zum nächsten slot gehen weil es ja voll ist und der name ist gleich
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
            //ResetItemDescription();
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
            playerMovement.enabled = !isActive;
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

    // Reset der rechten Beschreibung (Bild, Name, Text)
    private void ResetItemDescription()
    {
        foreach (var slot in itemSlot)
        {
            slot.ResetDescription();
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
