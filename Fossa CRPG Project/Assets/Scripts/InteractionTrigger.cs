using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InteractionTrigger : MonoBehaviour
{
    private bool inCombat;
    public bool oneTimeUse;

    public GameObject inputUI;

    private bool used = false;
    public Interaction oneTimeInteraction;

    private bool active;
    public Interaction enableInteraction;
    public Interaction disableInteraction;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    private void OnTriggerStay(Collider other)
    {
        if (inCombat || !other.gameObject.CompareTag("Player")) { return; }
        if (Input.GetButtonDown("Interact") && !used)
        {
            if(oneTimeUse == true)
            {
                oneTimeInteraction.Invoke();
                used = true;
            }
            else if(enableInteraction != null && disableInteraction != null)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //SHOW UI
            inputUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
}

[Serializable]
public class Interaction : UnityEvent { }