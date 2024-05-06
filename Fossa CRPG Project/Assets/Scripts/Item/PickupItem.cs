using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    //This script goes on the item
    public Item itemData;

    private HeldItem OscarHeldItem;
    public PlayerMovement pMovement;

    public bool held;

    public JournalEntryData journalEntry;
    [HideInInspector] public bool hasBeenPickedUp;

    [HideInInspector] public Transform areaParent;

    public override void CallInteraction()
    {
        //Check if Oscar is holding a weapon first
        if (OscarHeldItem != null && !held)
        {
            if (OscarHeldItem.currentItem != null) { OscarHeldItem.DropItem(); }
            OscarHeldItem.HoldNewItem(itemData);

            pMovement.nearbyInteractions.Remove(this);

            held = true;

            disableSmell();
        }

        InteractionManager.current.PickupItem(this);
        
    }

    public override void enableSmell()
    {
        if (!smellParticles.isPlaying && !held)
        {
            smellParticles.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inCombat && !inDialogue && other.TryGetComponent(out PlayerMovement pm) && !held)
        {
            if (!pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Add(this);
            }

            if (OscarHeldItem == null) { OscarHeldItem = other.gameObject.GetComponent<HeldItem>(); }
            if(pMovement == null) { pMovement = pm; }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!inCombat && !inDialogue && other.gameObject.TryGetComponent(out PlayerMovement pm) && !held)
        {
            if (pm.nearbyInteractions.Contains(this))
            {
                pm.nearbyInteractions.Remove(this);
            }
        }
    }
}

public enum ItemType
{
    Weapon = 0,
    Consumable = 1, 
    nonCombat = 2
}