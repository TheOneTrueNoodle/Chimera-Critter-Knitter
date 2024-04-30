using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    [SerializeField] private Transform itemParent;
    public Item currentItem;

    private void Update()
    {
        if (currentItem != null && Input.GetButtonDown("Drop Item"))
        {
            DropItem();
        }
    }

    public void HoldNewItem(Item item)
    {
        Debug.Log("Picking up Item");
        currentItem = item;

        //Disable any object physics
        if (currentItem.rb != null)
        {
            currentItem.disablePhysics();
        }

        //Put Held Item In Mouth
        currentItem.gameObject.transform.parent = itemParent;
        currentItem.gameObject.transform.localPosition = Vector3.zero;
        currentItem.gameObject.transform.localRotation = Quaternion.Euler(currentItem.holdRotationV3);

        //Handle Item Data
        switch (item.itemType)
        {
            case ItemType.Weapon:
                GetComponent<Entity>().heldWeapon = item.gameObject.GetComponent<WeaponItem>().weaponData;
                break;
            case ItemType.Consumable:
                GetComponent<Entity>().heldConsumableItem = item.gameObject.GetComponent<ConsumableItem>();
                break;
            case ItemType.nonCombat:
                break;
        }
    }

    public void DropItem()
    {
        Debug.Log("Dropping Item");

        //Drop Item From parent
        currentItem.gameObject.transform.parent = null;
        currentItem.GetComponent<PickupItem>().held = false;

        //Re enable any object physics
        if (currentItem.rb != null)
        {
            currentItem.enablePhysics();
        }

        switch (currentItem.itemType)
        {
            case ItemType.Weapon:
                GetComponent<Entity>().heldWeapon = null;
                break;
            case ItemType.Consumable:
                GetComponent<Entity>().heldConsumableItem = null;
                break;
            case ItemType.nonCombat:
                break;
        }

        currentItem = null;
    }
}
