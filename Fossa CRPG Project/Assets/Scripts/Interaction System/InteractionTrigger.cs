using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InteractionTrigger : Interactable
{
    public override void CallInteraction()
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

    private void OnTriggerEnter(Collider other)
    {
        if (!inCombat && !inDialogue && other.TryGetComponent(out PlayerMovement pm) && !used)
        {
            if (!pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((!inCombat && !inDialogue && other.TryGetComponent(out PlayerMovement pm)))
        {
            if (pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Remove(this);
            }
        }
    }
}