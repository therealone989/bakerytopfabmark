using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuActivated;


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
            // Time.timeScale = 0;  // OPTIONAL -- STOPPT ZEIT BEI AKTIVEN MENÜ - Physics Stoppen auch, Animations können Fehler geben
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }
}
