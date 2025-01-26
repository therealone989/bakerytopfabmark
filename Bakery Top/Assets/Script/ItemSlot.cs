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
            Debug.Log("ItemSlot ist voll (übergebenene Menge = Maximal im Slot verfügbare Plätze)");
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

        return 0;  // Keine Überreste
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
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
    public void OnRightClick()
    {
        
    }


}
