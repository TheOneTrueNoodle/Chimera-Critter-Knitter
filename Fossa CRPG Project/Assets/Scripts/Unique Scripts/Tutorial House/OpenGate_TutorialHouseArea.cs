using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate_TutorialHouseArea : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject interactionTrigger;
    [SerializeField] private DialogueZoneTrigger dontHaveKeyDialogue;
    [SerializeField] private DialogueZoneTrigger notInfectedDialogue;

    public void Call()
    {
        if (AreaManager.current.GetComponent<TutorialHouseArea>().areaBools.ContainsKey("Has gate key") && AreaManager.current.GetComponent<TutorialHouseArea>().areaBools["Has gate key"] == true)
        {
            //Has the gate key, so open
            anim.enabled = true;
            Destroy(dontHaveKeyDialogue.gameObject);
            interactionTrigger.SetActive(false);
        }
        else if(AreaManager.current.GetComponent<TutorialHouseArea>().areaBools.ContainsKey("Infected") && AreaManager.current.GetComponent<TutorialHouseArea>().areaBools["Infected"] == true)
        {
            //Dialogue for needing the gate key if infected
            dontHaveKeyDialogue.DialogueButtonTrigger();
        }
        else
        {
            //Woof?
            notInfectedDialogue.DialogueButtonTrigger();
        }
    }
}
