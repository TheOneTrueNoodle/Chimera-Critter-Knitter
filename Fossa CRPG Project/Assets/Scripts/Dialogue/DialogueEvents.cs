using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    public static DialogueEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onStartDialogue;
    public void StartDialogue()
    {
        if (onStartDialogue != null)
        {
            onStartDialogue();
        }
    }

    public event Action onEndDialogue;
    public void EndDialogue()
    {
        if (onEndDialogue != null)
        {
            onEndDialogue();
        }
    }
}
