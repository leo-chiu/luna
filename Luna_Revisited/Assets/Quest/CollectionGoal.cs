using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectionGoal : Goal
{
    public int item_id;
     
    public CollectionGoal(int item_id, int quest_id, string description, bool completed, int needed_amount, int current_amount)
    {
        this.item_id = item_id;
        this.quest_id = quest_id;
        this.description = description;
        this.completed = completed;
        this.needed_amount = needed_amount;
        this.current_amount = current_amount;
    }

    public override void Init()
    {
        Inventory.instance.onItemCallback += Collected;
        Inventory.instance.item_store.TryGetValue(item_id, out current_amount);
        if(current_amount >= needed_amount)
        {
            Collected();
        }
    }

    public void Collected()
    {
        if (Inventory.instance.item_store.TryGetValue(item_id, out int new_amount)) {
            current_amount = new_amount;
        }
        else
        {
            current_amount = 0;
        }

        Evaluate();
        QuestManager.instance.OnQuestProgress(quest_id);
    }

    public override void Complete()
    {
        base.Complete();
        Debug.Log("Completed collection goal of " + description);
    }
}
