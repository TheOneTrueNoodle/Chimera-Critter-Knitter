using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Unit_Stats", menuName = "Combat/Units/New Unit Stats", order = 1)]
public class UnitStats : ScriptableObject
{
    public int MovementSpeed;
    [Space(0)]
    public float MaxHP;
    public float MaxSP;
    public float Attack;
    public float Defence;
    public float MagicAttack;
    public float MagicDefence;
    public float CriticalChance;
    public float Speed;
    public float Accuracy;
    public float Dodge;
}

