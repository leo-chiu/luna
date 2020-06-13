using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; set; }

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public Queue<Dialogue> dialogue_queue;
    public Dialogue current_dialogue;
    public Speech current_line;
    public string current_speaker;

    public CanvasGroup panel_canvas_group;
    public float fade_speed;

    public GameObject dialogue_panel;
    public TextMeshProUGUI dialogue_box;

    public TextMeshProUGUI speaker_name;

    public GameObject interogatives;

    public event Action onGivenDialogue;
    public event Action onEndDialogue;

    public event Action onAccept;
    public event Action onDecline;

    public void Start()
    {
        dialogue_queue = new Queue<Dialogue>();
        panel_canvas_group = dialogue_panel.GetComponent<CanvasGroup>();
        current_dialogue = null;
        current_line = null;
        onGivenDialogue += displayDialogue;
        onEndDialogue += hideDialogue;
        onEndDialogue += hideInterogatives;
    }

    public void addDialogue(Dialogue dialogue)
    {
        dialogue_queue.Enqueue(dialogue);

        // only immediately override the dialogue and line if there is only one dialogue element in the queue, the inital one
        if (dialogue_queue.Count == 1 && current_dialogue == null && current_line == null)
        {
            current_dialogue = getNextDialogue();
            current_line = getNextLine();
        }
        
        if(onGivenDialogue != null)
        {
            onGivenDialogue.Invoke();
        }
    }

    public Dialogue getNextDialogue()
    {
        if (isEmpty()) return null;
        Dialogue dialogue = dialogue_queue.Peek();
        dialogue_queue.Dequeue();
        return dialogue;
    }

    public Speech getNextLine()
    {
        if (current_dialogue == null) return null;
        return current_dialogue.getLine();
    }

    public Dialogue peekNextDialogue()
    {
        if (isEmpty()) return null;
        return dialogue_queue.Peek();
    }

    public string getCurrentSpeaker()
    {
        if(current_dialogue != null)
        {
            return current_dialogue.speaker;
        }
        return null;
    }

    public void forward()
    {
        // disallows the player to move the dialogue forward using the designated key when prompted a question
        if (current_line != null && current_line.type == SentenceType.Interogative) return;

        if(current_dialogue == null || current_dialogue.isEmpty())
        {
            current_dialogue = getNextDialogue();
        }

        current_line = getNextLine();
        current_speaker = getCurrentSpeaker();

        assignLineToUI(current_line);

        if(current_line == null && current_dialogue == null)
        {
            if(onEndDialogue != null)
            {
                onEndDialogue.Invoke();
            }
        }
    }

    public bool isEmpty()
    {
        return dialogue_queue.Count == 0;
    }

    public void assignLineToUI(Speech dialogue_line)
    {
        if (dialogue_line == null)
        {
            hideDialogue();
            return;
        }
        dialogue_box.text = dialogue_line.words;
        speaker_name.text = getCurrentSpeaker();

        if (dialogue_line.type == SentenceType.Interogative)
        {
            caseInterogative();
        }
    }

    public void displayDialogue()
    {
        assignLineToUI(current_line);
        dialogue_panel.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void hideDialogue()
    {
        StartCoroutine(FadeOut());
    }

    public void caseInterogative()
    {
        interogatives.SetActive(true);
    }

    public void hideInterogatives()
    {
        interogatives.SetActive(false);
    }

    public void Accept()
    {
        if(onAccept != null)
        {
            onAccept.Invoke();
        }
    }

    public void Decline()
    {
        if(onDecline != null)
        {
            onDecline.Invoke();
        }
    }

    public IEnumerator FadeIn()
    {
        for(panel_canvas_group.alpha = 0; panel_canvas_group.alpha < 1; panel_canvas_group.alpha += fade_speed)
        {
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        for (panel_canvas_group.alpha = 1; panel_canvas_group.alpha > 0; panel_canvas_group.alpha -= fade_speed)
        {
            yield return null;
        }
        hideInterogatives();
        dialogue_panel.SetActive(false);
    }
}
