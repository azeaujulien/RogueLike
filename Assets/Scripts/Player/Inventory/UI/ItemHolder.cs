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

    public void UpdateAmountGraphics()
    {
        amountText.text = item.GetNumberInInventory().ToString();
    }
    
    public void ChangeItem(Item newItem)
    {
        item = newItem;
        InitGraphics();
    }

    public void UseItem()
    {
        item.Action();
    }
}
