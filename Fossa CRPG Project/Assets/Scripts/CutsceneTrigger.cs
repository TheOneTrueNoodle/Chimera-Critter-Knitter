using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject cutsceneObject;

    public void Call()
    {
        cutsceneObject.SetActive(true);
        DialogueEvents.current.StartDialogue();
    }
}
