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
    public bool isFull;


    // ====== ITEM SLOT ======= //
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite sprite)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = sprite;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = sprite;
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
    }
    public void OnRightClick()
    {
        
    }


}
