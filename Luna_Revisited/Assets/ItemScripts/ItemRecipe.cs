using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemRecipe : ScriptableObject
{
    public Item item;
    public int production_count;
    public List<KeyValuePair<Item, int>> recipe;
}
