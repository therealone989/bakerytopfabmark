using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;


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

    public void AddItem(string itemName, int quantity, Sprite sprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, sprite);
                return;
            }
        }
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
