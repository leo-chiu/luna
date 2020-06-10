using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")] [System.Serializable]
public class Item : ScriptableObject
{
    new public string name;
    public int item_id;

    [TextArea(3,1)]
    public string description;

    public Sprite sprite;

    public bool is_default_item;

    public int stack_capacity;

    public virtual void Use()
    {
        Debug.Log("Use");
    }
}
