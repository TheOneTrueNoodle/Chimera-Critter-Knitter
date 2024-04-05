using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public AbilityData itemAbility;

    private void Start()
    {
        itemType = ItemType.Consumable;
    }

    public void UseItem()
    {

    }
}
