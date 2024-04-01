using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkTrigger : Interactable
{

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
}
