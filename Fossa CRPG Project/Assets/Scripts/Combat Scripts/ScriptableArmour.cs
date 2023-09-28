using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "New Armour", menuName = "Combat/Equipment/Armour", order = 2)]
public class ScriptableArmour : ScriptableObject
{
    public string armourName;

    public bool characterSpecific;
    public string characterName;

    public List<EquipmentStatChanges> StatChanges;
    public List<DamageTypes> Resistances;
    public List<DamageTypes> Weaknesses;
    public List<ScriptableEffect> AdditionalEffects;
}
