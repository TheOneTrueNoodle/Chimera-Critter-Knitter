using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability", order = 2)]
public class AbilityData : ScriptableObject
{
    [Header("General Information")]
    public string Name;

    public string Desc;

    public string AnimationName;

    public Sprite symbol;

    public GameObject abilityVisual;

    public Sprite mutationMenuVisual;

    [Header("Ability Information")]
    public TextAsset abilityShape;

    public List<ScriptableEffect> effects;

    public int range;

    public int cooldown;
    [HideInInspector] public int TurnsSinceUsed;

    public CostTypes costType;

    public int abilityCost;

    public DamageTypes damageType;

    public int value;

    public bool addMeleeAttack;

    public AbilityTypes abilityType;

    public float requiredLevel;

    public bool requiresTarget;

    public bool includeCenter;

    public enum AbilityTypes
    {
        Enemy,
        Ally,
        All
    }

    public enum CostTypes
    {
        SP,
        HP
    }
}