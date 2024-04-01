using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InteractionTrigger : Interactable
{
    public GameObject inputUI;

    private void OnTriggerStay(Collider other)
    {
        if (inCombat|| inDialogue || !other.gameObject.CompareTag("Player")) { return; }
        if (Input.GetButtonDown("Interact") && !used)
        {
            if(oneTimeUse)
            {
                singleInteraction.Invoke();
                used = true;
            }
            else if(enableInteraction.GetPersistentEventCount() > 0 && disableInteraction.GetPersistentEventCount() > 0)
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

    private void OnTriggerEnter(Collider other)
    {
        if ((inCombat || inDialogue || other.gameObject.CompareTag("Player")))
        {
            //SHOW UI
            inputUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((inCombat || inDialogue || other.gameObject.CompareTag("Player")))
        {
            //HIDE UI
            inputUI.SetActive(false);
        }
    }
}