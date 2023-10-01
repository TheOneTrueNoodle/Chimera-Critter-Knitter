using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

[CreateAssetMenu(fileName = "Body Part", menuName = "Creatures/Parts", order = 2)]
public class CreatureBodyPart : ScriptableObject
{
    public BodyPart partType;
    public List<ScriptableEffect> mutations;
    public List<EquipmentStatChanges> statMutations;
    public List<AbilityData> abilities;

    public Mesh[] mesh;
    public Material[] materials;
}
