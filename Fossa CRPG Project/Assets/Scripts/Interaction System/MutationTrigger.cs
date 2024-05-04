using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MutationTrigger : Interactable
{
    public AbilityData requiredMutation;

    public Animator wrongMutationAnim;

    private PlayerMovement playerMovement;

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
            if (mutationMenu.unlockedAbilities.Contains(requiredMutation))
            {
                MenuEvent.current.SpawnPopup("You must equip the correct mutation");
            }
            else
            {
                MenuEvent.current.SpawnPopup("You have not discovered this mutation");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inDialogue && other.TryGetComponent(out PlayerMovement pm) && !used)
        {
            if (playerMovement == null) { playerMovement = pm; }

            if (!pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Add(this);
            };
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement pm))
        {
            if (playerMovement == null) { playerMovement = pm; }

            if (pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Remove(this);
            }
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null && playerMovement.nearbyInteractions.Contains(this))
        {
            playerMovement.nearbyInteractions.Remove(this);
        }
    }
}