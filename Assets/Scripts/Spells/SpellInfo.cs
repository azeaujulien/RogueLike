using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellInfo : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    
    public void InitGraphics(Spell spell)
    {
        icon.sprite = spell.spellImage;
        nameText.text = spell.spellName;
        descriptionText.text = spell.spellDescription;
        priceText.text = "Price : " + spell.spellPrice;
    }
}
