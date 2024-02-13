using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InteractionTrigger : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;
    public bool oneTimeUse;

    public GameObject inputUI;

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
        if (inCombat|| inDialogue || !other.gameObject.CompareTag("Player")) { return; }
        if (Input.GetButtonDown("Interact") && !used)
        {
            if(oneTimeUse == true)
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
        if (inCombat || inDialogue || other.gameObject.CompareTag("Player"))
        {
            //SHOW UI
            inputUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inCombat || inDialogue || other.gameObject.CompareTag("Player"))
        {
            //HIDE UI
            inputUI.SetActive(false);
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

[Serializable]
public class Interaction : UnityEvent { }