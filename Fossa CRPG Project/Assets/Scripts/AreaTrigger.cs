using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;
    public bool oneTimeUse; 
    
    private bool used = false;
    public Interaction Interaction;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        if (DialogueEvents.current != null) { DialogueEvents.current.onStartDialogue += StartDialogue; }
        if (DialogueEvents.current != null) { DialogueEvents.current.onEndDialogue += EndDialogue; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (inCombat || inDialogue) { return; }
        if (!used && other.gameObject.CompareTag("Player"))
        {
            if (oneTimeUse == true)
            {
                Interaction.Invoke();
                used = true;
            }
            else
            {
                Interaction.Invoke();
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
