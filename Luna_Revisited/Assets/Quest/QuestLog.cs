using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestLog : MonoBehaviour
{
    public List<Quest> quests;

    public void Add(Quest quest)
    {
        quests.Add(quest);
        quests[quests.IndexOf(quest)].Init();
    }
}
