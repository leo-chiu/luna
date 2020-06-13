using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance { get; set; }
    public Dictionary<int, Item> all_items;
    public Sprite default_sprite;

    public float bobbing_speed;

    public float scatter_factor;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            all_items = new Dictionary<int, Item>();
            Fill_Item_Catalogue();
        }
    }

    public void Fill_Item_Catalogue()
    {

        Item[] items = Resources.LoadAll<Item>("ItemAssets/Items");
        for(int i = 0; i < items.Length; i++)
        {
            if (!all_items.ContainsKey(items[i].item_id))
            {
                if (items[i].sprite == null)
                {
                    //Debug.Log("No sprite found for " + clone.name + ", using default sprite");
                    items[i].sprite = default_sprite;
                }
                
                all_items.Add(items[i].item_id, items[i]);
            }
        }
    }
}
