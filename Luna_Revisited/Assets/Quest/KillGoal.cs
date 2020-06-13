using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillGoal : Goal
{
    public int enemy_id;
    
    public KillGoal(int enemy_id, int quest_id, string description, bool completed, int needed_amount, int current_amount)
    {
        this.enemy_id = enemy_id;
        this.quest_id = quest_id;
        this.description = description;
        this.completed = completed;
        this.needed_amount = needed_amount;
        this.current_amount = current_amount;
    }

    public override void Init()
    {
        CombatEventManager.instance.onEnemyDeath += EnemyDied;
    }

    public void EnemyDied(int enemy_id)
    {
        if(this.enemy_id == enemy_id && !completed)
        {
            this.current_amount++;
            Evaluate();
        }
        QuestManager.instance.OnQuestProgress(quest_id);
    }

    public override void Complete()
    {
        base.Complete();
        Debug.Log("Completed kill goal of " + description);
    }
}
