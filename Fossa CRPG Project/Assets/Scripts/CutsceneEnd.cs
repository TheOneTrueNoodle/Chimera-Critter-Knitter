using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public List<GameObject> setActiveOnCutsceneEnd;
    public List<GameObject> setInactiveOnCutsceneEnd;

    public void Call()
    {
        DialogueEvents.current.EndDialogue();

        if (setActiveOnCutsceneEnd != null && setActiveOnCutsceneEnd.Count > 0)
        {
            foreach (GameObject obj in setActiveOnCutsceneEnd)
            {
                obj.SetActive(true);
            }
        }

        if (setInactiveOnCutsceneEnd != null && setInactiveOnCutsceneEnd.Count > 0)
        {
            foreach (GameObject obj in setInactiveOnCutsceneEnd)
            {
                obj.SetActive(false);
            }
        }
    }
}
