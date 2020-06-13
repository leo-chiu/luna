using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Speech
{
    [TextArea(5, 3)]
    public string words;
    public SentenceType type;

    public Speech(string words, SentenceType type)
    {
        this.words = words;
        this.type = type;
    }
}

public enum SentenceType { Declarative, Interogative }