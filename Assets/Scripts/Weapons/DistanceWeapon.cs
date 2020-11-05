using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="Other/Distance Weapon", order = 0)]
public class DistanceWeapon : Weapon
{
    [HideInInspector] public int currentAmmo;
    public int maxAmmo;

    private void OnValidate()
    {
        currentAmmo = maxAmmo;
    }
}
