using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public WeaponHolder[] weaponHolders = new WeaponHolder[2];
    public int currentWeaponIndex = 0;

    private void Start()
    {
        for (int i = 0; i < weaponHolders.Length; i++) {
            weaponHolders[i].InitGraphics();
            if (i == currentWeaponIndex) {
                weaponHolders[i].UseWeapon();
            } else {
                weaponHolders[i].UnUseWeapon();
            }
        }
    }

    private void Update()
    {
        int oldIndex = currentWeaponIndex;
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
        }

        if (currentWeaponIndex != oldIndex) {
            UpdateGraphics();
        }
    }

    private void UpdateGraphics()
    {
        for (int i = 0; i < weaponHolders.Length; i++) {
            if (i == currentWeaponIndex) {
                weaponHolders[i].UseWeapon();
            } else {
                weaponHolders[i].UnUseWeapon();
            }
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return weaponHolders[currentWeaponIndex].weapon;
    }
}
