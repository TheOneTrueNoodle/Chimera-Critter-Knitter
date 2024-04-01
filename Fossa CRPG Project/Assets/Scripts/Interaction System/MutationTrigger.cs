using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MutationTrigger : Interactable
{
    public AbilityData requiredMutation;

    public ParticleSystem mutationParticles;
    public GameObject inputUI;
    public Animator wrongMutationAnim;

    private void OnTriggerStay(Collider other)
    {
        if (inCombat || inDialogue || !other.gameObject.CompareTag("Player")) { return; }
        if (Input.GetButtonDown("Interact") && !used)
        {
            //Check if player has the correct ability
            bool hasMutationEquipped = false;
            MutationMenu mutationMenu = FindObjectOfType<MutationMenu>();
            foreach (AbilityData mutation in mutationMenu.equippedMutations)
            {
                if (mutation.Name == requiredMutation.Name)
                {
                    hasMutationEquipped = true;
                    break;
                }
            }

            if (hasMutationEquipped)
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
            else
            {
                wrongMutationAnim.Play("MissingMutation");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((inCombat || inDialogue || other.gameObject.CompareTag("Player")))
        {
            //SHOW UI
            inputUI.SetActive(true);
            mutationParticles.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((inCombat || inDialogue || other.gameObject.CompareTag("Player")))
        {
            //HIDE UI
            inputUI.SetActive(false);
            mutationParticles.Stop();
        }
    }
}