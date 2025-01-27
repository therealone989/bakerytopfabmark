using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    public ItemSO[] itemSOs;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory") && menuActivated)
        {
            // Time.timeScale = 1;  // OPTIONAL -- AKTIVIERT WIEDER ZEIT
            InventoryMenu.SetActive(false);
            menuActivated = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            // Time.timeScale = 0;  // OPTIONAL -- STOPPT ZEIT BEI AKTIVEN MENÜ - Physics Stoppen auch, Animations können Fehler geben
            InventoryMenu.SetActive(true);
            menuActivated = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
}
