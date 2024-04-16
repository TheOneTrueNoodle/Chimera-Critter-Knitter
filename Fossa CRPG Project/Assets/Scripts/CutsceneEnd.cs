using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public void Call()
    {
        DialogueEvents.current.EndDialogue();
    }
}
