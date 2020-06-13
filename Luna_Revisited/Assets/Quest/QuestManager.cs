using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance { set; get; }

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public Dictionary<int, Quest> all_quests;

    public void Start()
    {
        all_quests = new Dictionary<int, Quest>();
        initialize_all_quests();
    }

    public void initialize_all_quests()
    {
        Quest[] loaded_in = Resources.LoadAll<Quest>("Quests");
        foreach(Quest q in loaded_in)
        {
            if (!all_quests.ContainsKey(q.quest_id)) {
                Quest copy_quest = (Quest)ScriptableObject.CreateInstance<Quest>();

                q.duplicate_quest(ref copy_quest);

                all_quests.Add(q.quest_id, copy_quest);
            }
        }
    }

    public event Action<int> onQuestProgress;

    public void OnQuestProgress(int quest_id)
    {
        if (onQuestProgress != null)
        {
            onQuestProgress.Invoke(quest_id);
        }
    }

    public event Action<int> onQuestCompleted;

    public void OnQuestCompleted(int quest_id)
    {
        if (onQuestCompleted != null)
        {
            onQuestCompleted.Invoke(quest_id);
        }
    }
}
