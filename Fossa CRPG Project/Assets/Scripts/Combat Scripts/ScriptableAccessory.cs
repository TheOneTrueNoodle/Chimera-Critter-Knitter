using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Combat/Equipment/Accessory", order = 3)]
public class ScriptableAccessory : ScriptableObject
{
    public string accessoryName;

    public bool characterSpecific;
    public string characterName;

    public List<EquipmentStatChanges> StatChanges;
    public List<ScriptableEffect> AdditionalEffects;
    public List<DamageTypes> Resistances;
    public List<DamageTypes> Weaknesses;
}
