using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkTrigger : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;

    private bool used = false;
    public Interaction oneTimeInteraction;

    private bool active;
    public Interaction enableInteraction;
    public Interaction disableInteraction;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        DialogueEvents.current.onStartDialogue += StartDialogue;
        DialogueEvents.current.onEndDialogue += EndDialogue;
    }

    private void OnTriggerStay(Collider other)
    {
        if (inCombat || inDialogue) { return; }
        if (Input.GetButton("Bark") && !used)
        {
            if (oneTimeInteraction != null)
            {
                oneTimeInteraction.Invoke();
                used = true;
            }
            else if (enableInteraction != null && disableInteraction != null)
            {
                if (active)
                {
                    disableInteraction.Invoke();
                    active = false;
                }
                else
                {
                    enableInteraction.Invoke();
                    active = true;
                }
            }
        }
    }
    private void StartCombat()
    {
        inCombat = true;
    }
    private void EndCombat()
    {
        inCombat = false;
    }
    private void StartDialogue()
    {
        inDialogue = true;
    }
    private void EndDialogue()
    {
        inDialogue = false;
    }
}
