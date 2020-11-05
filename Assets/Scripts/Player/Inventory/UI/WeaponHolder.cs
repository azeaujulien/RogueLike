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

    
    /// <summary>
    ///     This function init all UI info of weapon
    /// </summary>
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
    
    /// <summary>
    ///     This function change weapon and reinit UI
    /// </summary>
    /// <param name="newWeapon">New weapon who replace the current</param>
    public void ChangeWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        InitGraphics();
    }

    /// <summary>
    ///     This function update the number of ammo in UI
    /// </summary>
    public void UpdateAmmoGraphics()
    {
        DistanceWeapon info = weapon as DistanceWeapon;
        if (info != null) {
            ammoText.text = info.currentAmmo + "/" + info.maxAmmo;
        } else {
            ammoText.text = "";
        }
    }
    
    /// <summary>
    ///     This function set visual info to show the weapon in use
    /// </summary>
    public void UseWeapon()
    {
        background.color = useBackgroundColor;
    }

    /// <summary>
    ///     This function set visual info to show the weapon in use
    /// </summary>
    public void UnUseWeapon()
    {
        background.color = unuseBackgroundColor;
    }
}
