using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public Item item;
    public string item_name;
    public int item_id;
    public int count;
    
    public ItemStack(Item item, int count)
    {
        this.item = item;
        this.count = count;

        item_id = item.item_id;
        item_name = item.name;
    }

    public override string ToString()
    {
        return item.name + "(" + count + ")";
    }
}
