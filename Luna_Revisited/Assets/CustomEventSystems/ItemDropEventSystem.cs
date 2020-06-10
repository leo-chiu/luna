using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDropEventSystem : MonoBehaviour
{
    public static ItemDropEventSystem instance {get; set;}

    private void Awake()
    {
        instance = this;
    }

    public event Action<int> onItemPickup;
    public void ItemPickup(int item_instance_id)
    {
        onItemPickup.Invoke(item_instance_id);
    }
}
