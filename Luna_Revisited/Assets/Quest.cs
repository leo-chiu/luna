using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public new string name;
    public int quest_id;
    public string description;
    public bool completed;
    public List<KillGoal> kill_objectives;
    public List<CollectionGoal> collect_objectives;
    public List<ItemStack> rewards;

    public void duplicate_quest(ref Quest quest_copy)
    {
        quest_copy.kill_objectives = new List<KillGoal>();
        quest_copy.collect_objectives = new List<CollectionGoal>();
        quest_copy.rewards = new List<ItemStack>();

        quest_copy.name = name;
        quest_copy.quest_id = quest_id;
        quest_copy.description = description;
        quest_copy.completed = completed;

        foreach(KillGoal kg in kill_objectives)
        {
            quest_copy.kill_objectives.Add(new KillGoal(kg.enemy_id, kg.quest_id, kg.description, kg.completed, kg.needed_amount, kg.current_amount));
        }

        foreach (CollectionGoal cg in collect_objectives)
        {
            quest_copy.collect_objectives.Add(new CollectionGoal(cg.item_id, cg.quest_id, cg.description, cg.completed, cg.needed_amount, cg.current_amount));
        }

        foreach(ItemStack i in rewards)
        {
            quest_copy.rewards.Add(i);
        }
    }

    public void Init()
    {
        foreach(KillGoal kg in kill_objectives)
        {
            kg.Init();
        }
        foreach(CollectionGoal cg in collect_objectives)
        {
            cg.Init();
        }
        QuestManager.instance.onQuestProgress += Evaluate;
        QuestManager.instance.onQuestCompleted += Reward;
    }

    public void Evaluate(int quest_id)
    {
        if (this.quest_id == quest_id)
        {
            bool all_completed = true;
            foreach (KillGoal kg in kill_objectives)
            {
                if (!kg.completed)
                {
                    all_completed = false;
                }
            }
            foreach (CollectionGoal cg in collect_objectives)
            {
                if (!cg.completed)
                {
                    all_completed = false;
                }
            }

            if (all_completed)
            {
                Complete();
            }
        }
    }

    public void Complete()
    {
        completed = true;
        Debug.Log("Quest Completed!");
        QuestManager.instance.OnQuestCompleted(quest_id);
    }

    public void Reward(int quest_id)
    {
        if (this.quest_id == quest_id)
        {
            if (Inventory.instance.canPickupItems(rewards))
            {
                foreach (ItemStack i in rewards)
                {
                    Inventory.instance.recieveItem(i);
                }
            }
            else
            {
                completed = false;
                Debug.Log("Insufficient Space in Inventory");
            }
        }
    }
}
