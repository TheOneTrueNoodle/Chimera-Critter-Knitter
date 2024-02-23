using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkTrigger : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;
    public bool oneTimeUse;

    private bool used = false;
    public Interaction singleInteraction;

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
        if (inCombat || inDialogue || !other.gameObject.CompareTag("Player")) { return; }
        if (Input.GetButton("Bark") && !used)
        {
            if (oneTimeUse)
            {
                singleInteraction.Invoke();
                used = true;
            }
            else if (enableInteraction.GetPersistentEventCount() > 0 && disableInteraction.GetPersistentEventCount() > 0)
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
            else
            {
                singleInteraction.Invoke();
            }
        }
    }
    private void StartCombat(string combatName)
    {
        inCombat = true;
    }
    private void EndCombat(string combatName)
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
