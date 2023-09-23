using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum StatsList
    {
        MaxHP = 0,
        MaxSP = 1,
        Speed = 2,
        Dodge = 3,
        Attack = 4,
        Defence = 5,
        MagicAttack = 6,
        MagicDefence = 7,
        Accuracy = 8,
        Crit = 9
    }

    public enum Operation
    {
        Add = 0,
        Minus = 1,
        Multiply = 2,
        Divide = 4,
        AddByPercentage = 5,
        MinusByPercentage = 6
    }

    public enum DamageTypes
    {
        Smashing = 0,
        Slashing = 1,
        Stabbing = 2,
        Fire = 3,
        Water = 4,
        Earth = 5,
        Wind = 6,
        Lightning = 7,
        Holy = 8,
        Vile = 9,
        Positive = 10,
        Negative = 11,
        Cosmic = 12,
        Healing = 13
    }

    public enum WeaponType
    {
        Gauntlets = 0,
        Dagger = 1,
        Sword = 2,
        Greatsword = 3,
        Gun = 4,
        Bow = 5,
        Crossbow = 6,
        Staff = 7,
        Spear = 8,
        Axe = 9,
        Greataxe = 10,
        Hammer = 11,
        Greathammer = 12
    }
}

