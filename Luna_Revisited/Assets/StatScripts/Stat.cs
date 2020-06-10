using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    public int base_value;
    public int total_value;

    public List<int> modifiers = new List<int>();

    public int getValue()
    {
        int final_value = base_value;
        modifiers.ForEach(x => final_value += x);
        return final_value;
    }

    public void addModifier(int modifier)
    {
        if(modifier != 0)
        {
            modifiers.Add(modifier);
        }
        total_value = getValue();
    }

    public void removeModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
        total_value = getValue();
    }
}
