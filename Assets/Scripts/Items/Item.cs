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

    /// <summary>
    ///     This function run the chosen action
    /// </summary>
    public void Action()
    {
        if (isUsable && numberInInventory > 0) {
            Debug.Log("Use " + itemName);
            numberInInventory -= 1;
        } else {
            Debug.Log("Item not usable");
        }
    }

    /// <summary>
    ///     This function add items in inventory
    /// </summary>
    /// <param name="amount">Amount of item to add</param>
    public void Add(int amount) => numberInInventory += amount;
    
    /// <summary>
    ///     This function remove items of inventory
    /// </summary>
    /// <param name="amount">Amount of item to remove</param>
    public void Remove(int amount) => numberInInventory -= amount;

    /// <summary>
    ///     This function return the number of item inventory
    /// </summary>
    /// <returns>
    ///    Number of item in inventory 
    /// </returns>
    public int GetNumberInInventory() => numberInInventory;
}
