using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : Interactable
{
    private void OnTriggerEnter(Collider other)
    {
        if (inCombat || inDialogue) { return; }
        if (!used && other.gameObject.CompareTag("Player"))
        {
            if (oneTimeUse == true)
            {
                singleInteraction.Invoke();
                used = true;
            }
            else if (enableInteraction.GetPersistentEventCount() > 0 && disableInteraction.GetPersistentEventCount() > 0)
            {
                if (!active)
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

    private void OnTriggerExit(Collider other)
    {
        if (inCombat || inDialogue) { return; }
        if (disableInteraction.GetPersistentEventCount() > 0 && other.gameObject.CompareTag("Player") && active)
        {
            disableInteraction.Invoke();
        }
    }
}
