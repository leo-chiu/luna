using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")][System.Serializable]
public class Dialogue : ScriptableObject
{
    public string speaker;

    [SerializeField]
    public List<Speech> dialogue = new List<Speech>();

    public void Init(Dialogue dialogue_obj)
    {
        List<Speech> dialogue = dialogue_obj.dialogue;

        for(int i = 0; i < dialogue.Count; i++)
        {
            this.dialogue.Add(dialogue[i]);
        }

        if (dialogue_obj.speaker == null || dialogue_obj.speaker == "")
        {
            this.speaker = "Unknown";
        }
        else
        {
            this.speaker = dialogue_obj.speaker;
        }
    }

    public void addLine(string dialogue_line, SentenceType type)
    {
        Speech to_add = new Speech(dialogue_line , type);
        dialogue.Add(to_add);
    }
    public Speech getLine()
    {
        Speech line = Peek();
        Dequeue();
        return line;
    }
    public bool isEmpty()
    {
        return dialogue.Count <= 0;
    }
    public void Dequeue()
    {
        if (!isEmpty()) dialogue.RemoveAt(0);
    }
    public Speech Peek()
    {
        if (!isEmpty()) return dialogue[0];
        return null;
    }
}
