using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestManager))] [System.Serializable]
public class QuestManagerEditor : Editor
{
    public void CreateQuestAssets()
    {
        //Debug.Log("Creating Quests from CSV");
        Quest quest = (Quest)ScriptableObject.CreateInstance("Quest");

        quest.name = "Take on the Hideout";
        quest.quest_id = 1;

        quest.kill_objectives = new List<KillGoal>();
        quest.collect_objectives = new List<CollectionGoal>();
        quest.rewards = new List<ItemStack>();

        Dictionary<string, int> item_key = new Dictionary<string, int>();

        // this item dictionary holds all items and their unique item ids
        Item[] items = Resources.LoadAll<Item>("ItemAssets/Items");

        foreach(Item i in items)
        {
            // it will act as our cache for loading in from a CSV format
            if (!item_key.ContainsKey(i.name))
            {
                item_key.Add(i.name, i.item_id);
            }
        }

        // from this point forward we can map inputs to unique item ids and pass them as goal arguments

        /* // Code that verifies that the dictionary contains all known items
        foreach(KeyValuePair<string, int> k in item_key)
        {
            Debug.Log(k.Key + "=>" + k.Value);
        }
        */

        // in the CSV we can determine whcih imperative to use for each goal; falls under quest design

        KillGoal kg = new KillGoal(0, 1, "Kill 5 hoodrats", false, 10, 0);

        quest.kill_objectives.Add(kg);

        KillGoal kg2 = new KillGoal(2, 1, "Kill 7 gang-members", false, 20, 0);

        quest.kill_objectives.Add(kg2);

        CollectionGoal cg = new CollectionGoal(1, 1, "Collect 2 wood", false, 2, 0);

        quest.collect_objectives.Add(cg);

        AssetDatabase.CreateAsset(quest, "Assets/Resources/Quests/newquest2.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(); 
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Generate Quests from CSV"))
        {
            CreateQuestAssets();
        }
    }
}
