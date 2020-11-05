using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="Other/Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponImage;
}
