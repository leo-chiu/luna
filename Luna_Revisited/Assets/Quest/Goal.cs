using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goal
{
    public int quest_id;
    public string description;
    public bool completed;
    public int needed_amount;
    public int current_amount;

    public virtual void Init()
    {
    }

    public virtual void Evaluate()
    {
        if(needed_amount <= current_amount)
        {
            Complete();
        }
    }

    public virtual void Complete()
    {
        completed = true;
    }
}
