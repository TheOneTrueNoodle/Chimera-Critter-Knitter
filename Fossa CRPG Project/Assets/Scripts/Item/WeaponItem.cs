using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    public ScriptableWeapon weaponData;

    private void Start()
    {
        itemType = ItemType.Weapon;
    }
}
