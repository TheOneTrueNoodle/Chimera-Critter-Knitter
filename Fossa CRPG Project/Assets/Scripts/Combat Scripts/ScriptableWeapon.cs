using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Combat/Equipment/Weapon", order = 1)]
public class ScriptableWeapon : ScriptableObject
{
    public string weaponName;

    public bool characterSpecific;
    public string characterName;

    public List<EquipmentStatChanges> StatChanges;
    public List<ScriptableEffect> AdditionalEffects;

    public WeaponType weaponType;
    public DamageTypes damageType;
    public int attackRange = 1;
    public int weaponDamage;
}
