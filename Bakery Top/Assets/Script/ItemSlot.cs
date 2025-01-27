using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // ====== ITEM DATEN ======= //
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public Sprite itemDescriptionImageSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField] private int maxNumberOfItems;


    // ====== ITEM SLOT ======= //
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public GameObject selectedShader;
    public bool thisItemSelected;


    // ====== ITEM BESCHREIBUNG SLOT ======= //
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;


    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        Debug.Log("ITEMSLOT SCRIPT");
    }

    public int AddItem(String itemName, int quantity, Sprite[] sprite, String itemDescription)
    {

        // Check to see if the slot is already full
        if(isFull)
        {
            return quantity;
        }

        // Update NAME
        this.itemName = itemName;

        // Update IMAGE
        this.itemSprite = sprite[0];
        itemImage.sprite = sprite[0];

        // Update ITEM DESCRIPTION
        this.itemDescription = itemDescription;

        // Update ITEM DESCRIPTION IMAGE (Right Side of Inventory)
        this.itemDescriptionImageSprite = sprite[1];

        // Update QUANTITY
        this.quantity += quantity;

        // WENN MEHR ANKOMMT ALS LIMIT ERLAUBT DANN 
        if(this.quantity >= maxNumberOfItems)
        {
            // ItemSlot ist voll (・ergebenene Menge = Maximal im Slot verf・bare Pl舩ze) 
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;
        
            // RETURN LEFTOVERS
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        // Update QUANTITY TEXT
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;  // Keine überreste
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("LEFT ONPINERCLECK");
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        Debug.Log("LEFT LCICKCKCKCKCK");
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemDescriptionImageSprite;
        if(itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

    private void EmptySlot()
    {
        Debug.Log("EMPTY");

        itemImage.sprite = emptySprite;
        itemName = "";
        itemSprite = emptySprite;
        quantityText.enabled = false;

        itemDescriptionImageSprite = emptySprite;
        ItemDescriptionText.text = "";
        itemDescription = "";

        // Setze das Flag für 'isFull' zurück, falls nötig
        isFull = false;
    }

    public void OnRightClick()
    {
        if (quantity <= 0) return; // Slot ist leer, nichts zu tun

        // Spawne das Item vor dem Spieler
        GameObject spawnedItem = SpawnItem();

        // Reduziere die Menge im Slot
        quantity--;
        quantityText.text = quantity > 0 ? quantity.ToString() : "";
        isFull = quantity > 0;
        if(this.quantity <= 0)
        {
            EmptySlot();
        }

        // Schließe das Inventar
        inventoryManager.ToggleInventory(false);

        // Übergib das gespawnte Item an den Grabber
        FindFirstObjectByType<Grabitem>().GrabObject(spawnedItem);
    }

    private GameObject SpawnItem()
    {
        // Spawne das Item vor dem Spieler
        Transform playerCamera = Camera.main.transform;
        Vector3 spawnPosition = playerCamera.position + playerCamera.forward * 1.5f;

        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/" + itemName);
        if (itemPrefab != null)
        {
            GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            return spawnedItem;
        }
        else
        {
            Debug.LogError("Item prefab not found for: " + itemName);
            return null;
        }
    }


}
