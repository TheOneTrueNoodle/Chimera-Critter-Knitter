using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    //This script goes on the item
    public Item itemData;

    public GameObject inputUI;
    private bool playerInArea;

    private HeldItem OscarHeldItem;

    public void PickupTrigger()
    {
        //Check if Oscar is holding a weapon first
        if (OscarHeldItem != null)
        {
            if(OscarHeldItem.currentItem != null) { OscarHeldItem.DropItem(); }
            OscarHeldItem.HoldNewItem(itemData);
            enabled = false;

            inputUI.SetActive(false);
        }
    }
    private void Update()
    {
        if (inCombat || inDialogue || !playerInArea) { return; }
        if (Input.GetButtonDown("Interact"))
        {
            PickupTrigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((inCombat || inDialogue || other.gameObject.CompareTag("Player")))
        {
            //SHOW UI
            inputUI.SetActive(true);
            playerInArea = true;
            if(OscarHeldItem == null) { OscarHeldItem = other.gameObject.GetComponent<HeldItem>(); }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((inCombat || inDialogue || other.gameObject.CompareTag("Player")))
        {
            //HIDE UI
            inputUI.SetActive(false);
            playerInArea = false;
        }
    }
}

public enum ItemType
{
    Weapon = 0,
    Consumable = 1, 
    nonCombat = 2
}