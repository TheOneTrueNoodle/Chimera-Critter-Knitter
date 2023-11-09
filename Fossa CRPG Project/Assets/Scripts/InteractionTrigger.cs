using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InteractionTrigger : MonoBehaviour
{
    public GameObject inputUI;

    private bool used = false;
    public Interaction oneTimeInteraction;

    private bool active;
    public Interaction enableInteraction;
    public Interaction disableInteraction;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && !used)
        {
            if(oneTimeInteraction != null)
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
}

[Serializable]
public class Interaction : UnityEvent { }