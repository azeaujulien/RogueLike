using UnityEngine;

public class PlayerHotbar : MonoBehaviour
{
    public ItemHolder[] itemHolders = new ItemHolder[5];
    
    private void Start()
    {
        foreach (ItemHolder holder in itemHolders) {
            holder.InitGraphics();
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            UseItem(itemHolders[0]);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            UseItem(itemHolders[1]);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            UseItem(itemHolders[2]);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            UseItem(itemHolders[3]);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            UseItem(itemHolders[4]);
        }
    }

    /// <summary>
    ///     This function use item in item holder and update graphics
    /// </summary>
    /// <param name="itemHolder">Item holder with wanted item in</param>
    private void UseItem(ItemHolder itemHolder)
    {
        itemHolder.UseItem();
        itemHolder.UpdateAmountGraphics();
    }
}
