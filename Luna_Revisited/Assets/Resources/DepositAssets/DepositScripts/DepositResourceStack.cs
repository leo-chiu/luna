using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DepositResourceStack
{
    // wrapper class that acts functionally as a key value pair of and item and count
    // enables serialization onto the inspector, improving pipeline and workflow
    public Item item;
    public int count;

    public DepositResourceStack(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}
