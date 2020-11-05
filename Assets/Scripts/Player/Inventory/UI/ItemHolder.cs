using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public Image background;
    public Image image;
    public TextMeshProUGUI amountText;
    [SerializeField] private Item item;

    /// <summary>
    ///     This function init all UI info of item
    /// </summary>
    public void InitGraphics()
    {
        image.sprite = item.itemImage;
        if (image.sprite == null) {
            image.color = new Color(255, 255, 255, 0);
            amountText.text = "";
        } else {
            image.color = new Color(255, 255, 255, 255);
            UpdateAmountGraphics();
        }
    }

    /// <summary>
    ///     This function update the text of the number of item in inventory
    /// </summary>
    public void UpdateAmountGraphics()
    {
        amountText.text = item.GetNumberInInventory().ToString();
    }
    
    /// <summary>
    ///     This function change item in holder and reinit the UI
    /// </summary>
    public void ChangeItem(Item newItem)
    {
        item = newItem;
        InitGraphics();
    }

    /// <summary>
    ///     This function run the action of the item
    /// </summary>
    public void UseItem()
    {
        item.Action();
    }
}
