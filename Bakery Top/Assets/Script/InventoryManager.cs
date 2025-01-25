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
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            // Time.timeScale = 0;  // OPTIONAL -- STOPPT ZEIT BEI AKTIVEN MEN� - Physics Stoppen auch, Animations k�nnen Fehler geben
            InventoryMenu.SetActive(true);
            menuActivated = true;
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
}
