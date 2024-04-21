using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MutationTrigger : Interactable
{
    public AbilityData requiredMutation;

    public ParticleSystem mutationParticles;
    public Animator wrongMutationAnim;

    public override void CallInteraction()
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

    private void OnTriggerEnter(Collider other)
    {
        if (!inDialogue && other.TryGetComponent(out PlayerMovement pm) && !used)
        {
            if (!pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Add(this);
            }

            mutationParticles.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!inCombat && !inDialogue && other.TryGetComponent(out PlayerMovement pm))
        {
            if (pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Remove(this);
            }
            mutationParticles.Stop();
        }
    }
}