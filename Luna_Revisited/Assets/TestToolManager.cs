using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToolManager : MonoBehaviour
{
    void Update()
    {
        // assign key inputs for functionality to debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Inventory.instance.RemoveItem(0, 4))
            {
                Debug.Log("Removed 4 of 0");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quest q = QuestManager.instance.all_quests[1];
            GetComponent<QuestLog>().Add(q);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            QuestManager.instance.OnQuestProgress(0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Dialogue dialogue = Resources.Load<Dialogue>("Dialogue/TestDialogue");
            Dialogue dialogue_copy = ScriptableObject.CreateInstance<Dialogue>();
            dialogue_copy.Init(dialogue);

            DialogueManager.instance.addDialogue(dialogue_copy);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Dialogue dialogue = Resources.Load<Dialogue>("Dialogue/TestDialogue2");
            Dialogue dialogue_copy = ScriptableObject.CreateInstance<Dialogue>();
            dialogue_copy.Init(dialogue);

            DialogueManager.instance.addDialogue(dialogue_copy);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            DialogueManager.instance.forward();
        }
    }
}
