using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // ====== ITEM DATEN ======= //
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;


    // ====== ITEM SLOT ======= //
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

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
}
