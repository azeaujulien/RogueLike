using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="Other/Item", order = 0)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    [SerializeField] private int numberInInventory;
    [SerializeField] private bool isUsable;

    public void Action()
    {
        if (isUsable && numberInInventory > 0) {
            Debug.Log("Use " + itemName);
            numberInInventory -= 1;
        } else {
            Debug.Log("Item not usable");
        }
    }

    public void Add(int amount) => numberInInventory += amount;
    
    public void Remove(int amount) => numberInInventory -= amount;

    public int GetNumberInInventory() => numberInInventory;
}
