using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCombatItem : Item
{
    private void Start()
    {
        itemType = ItemType.nonCombat;
    }
}
