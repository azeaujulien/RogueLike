using UnityEngine;

[CreateAssetMenu(fileName="New Spell", menuName="Other/Spell", order = 0)]

public class Spell : ScriptableObject
{
    public string spellName;
    public Sprite spellImage;
    public int spellPrice;
    public string spellDescription;
}
