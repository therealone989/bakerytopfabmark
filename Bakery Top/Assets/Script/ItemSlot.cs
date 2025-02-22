﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Daten")]
    // ====== ITEM DATEN ======= //
    public string itemName;
    public int quantity;
    [SerializeField] private int maxNumberOfItems;
    public Sprite itemSprite;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public bool isFull;

    // ====== ITEM DESCRIPTION SLOT ======= //
    [Header("Item Description Daten")]
    public Sprite itemDescriptionImageSprite;
    public string itemDescription;
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;


    // DEFAULT SPRITES
    [Header("Defaults")]
    public Sprite emptySprite;
    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(String itemName, int quantity, Sprite[] sprite, String itemDescription)
    {

        // Check to see if the slot is already full
        if (isFull)
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
        if (this.quantity >= maxNumberOfItems)
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
        if (eventData.button == PointerEventData.InputButton.Left)
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
        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

    private void EmptySlot()
    {

        // ITEMSLOT LEEREN
        itemImage.sprite = emptySprite;
        itemName = "";
        itemSprite = emptySprite;
        quantityText.enabled = false;

        // DESCRIPTION LEEREN
        ResetDescription();

        // Nicht mehr voll, isfull = false
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
        isFull = quantity >= maxNumberOfItems;
        Debug.Log(isFull);

        if (this.quantity <= 0)
        {
            Debug.Log("EMPTY ALL");
            EmptySlot();
        }

        // Schließe das Inventar
        inventoryManager.ToggleInventory(false);

        Grabitem grabItemScript = FindFirstObjectByType<Grabitem>();
        // Übergib das gespawnte Item an den Grabber
        grabItemScript.GrabObject(spawnedItem);
        grabItemScript.isGrabbing = true;

    }

    private GameObject SpawnItem()
    {
        // Spawne das Item vor dem Spieler
        Transform playerCamera = Camera.main.transform;
        Vector3 spawnPosition = playerCamera.position + playerCamera.forward * 1.5f;

        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/" + itemName);
        if (itemPrefab != null)
        {
            GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, itemPrefab.transform.rotation);
            return spawnedItem;
        }
        else
        {
            Debug.LogError("Item prefab not found for: " + itemName);
            return null;
        }
    }

    public void ResetDescription()
    {
        Debug.Log("REST");
        selectedShader.SetActive(false);
        thisItemSelected = false;

        itemDescriptionImageSprite = null;
        itemDescription = "";

        itemDescriptionImage.sprite = emptySprite;
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";

    }


}