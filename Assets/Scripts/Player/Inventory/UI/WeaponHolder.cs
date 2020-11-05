using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ammoText;
    
    public Image background;
    public Weapon weapon;
    
    
    public Color useBackgroundColor;
    public Color unuseBackgroundColor;

    public void InitGraphics()
    {
        nameText.text = weapon.weaponName;
        image.sprite = weapon.weaponImage;
        if (image.sprite == null) {
            image.color = new Color(255, 255, 255, 0);
        } else {
            image.color = new Color(255, 255, 255, 255);
        }

        UpdateAmmoGraphics();
    }
    
    public void ChangeWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        InitGraphics();
    }

    public void UpdateAmmoGraphics()
    {
        DistanceWeapon info = weapon as DistanceWeapon;
        if (info != null) {
            ammoText.text = info.currentAmmo + "/" + info.maxAmmo;
        } else {
            ammoText.text = "";
        }
    }

    public void UseWeapon()
    {
        background.color = useBackgroundColor;
    }

    public void UnUseWeapon()
    {
        background.color = unuseBackgroundColor;
    }
}
