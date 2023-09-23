using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class Stat
{
    public const float statModifier = 1.2f;

    public string name;
    public float baseStatValue;
    public float statValue;
    public bool isModified;
    public List<StatModifier> statMods = new List<StatModifier>();

    public Stat(string name, float statValue)
    {
        this.name = name;
        this.statValue = statValue;
        baseStatValue = statValue;
        isModified = false;
    }

    public void updateStat(float newValue)
    {
        statValue = newValue;
    }
    public void ApplyStatMod(Entity entity)
    {
        foreach (var statMod in statMods)
        {
            switch (statMod.op)
            {
                case Operation.Add:
                    statMod.changeValue = statMod.value;
                    break;
                case Operation.Minus:
                    statMod.changeValue = -statMod.value;
                    break;
                case Operation.Multiply:
                    statMod.changeValue = (statValue * statMod.value) - statValue;
                    break;
                case Operation.Divide:
                    statMod.changeValue = (statValue / statMod.value) - statValue;
                    break;
                case Operation.AddByPercentage:
                    statMod.changeValue = (baseStatValue * (statMod.value / 100));
                    break;
                case Operation.MinusByPercentage:
                    statMod.changeValue = -(baseStatValue * (statMod.value / 100));
                    break;
            }

            statValue += statMod.changeValue;
            if (name == "HP")
            {
                if (statValue > baseStatValue)
                {
                    statValue = baseStatValue;
                }

                Debug.Log("HP = " + statValue);
                if (statMod.changeValue > 0)
                {
                    Color color = Color.green;
                    CombatEvents.current.DamageDealt(((int)statMod.changeValue), entity, statMod.damageType);
                }
                if (statMod.changeValue < 0)
                {
                    Color color = Color.red;
                    CombatEvents.current.DamageDealt(((int)statMod.changeValue), entity, statMod.damageType);
                }

                entity.updateHealthBar();
                if (statValue <= 0)
                {
                    CombatEvents.current.UnitDeath(entity);
                }
            }
        }

        statMods.RemoveAll(x => x.duration <= 0);

        if (statMods.Count == 0) { isModified = false; }
        else { isModified = true; }
    }
    public void UpdateStatMods(Entity entity)
    {
        foreach (var statMod in statMods)
        {
            statMod.duration--;

            if (name != "HP" && name != "SP")
            {
                if (statMod.duration == 0)
                {
                    statValue -= statMod.changeValue;
                }
            }
            else if (name == "HP")
            {
                statValue += statMod.changeValue;

                if (statValue > baseStatValue)
                {
                    statValue = baseStatValue;
                }
                Debug.Log("HP = " + statValue);

                if (statMod.changeValue > 0)
                {
                    Color color = Color.green;
                    CombatEvents.current.DamageDealt(((int)statMod.changeValue), entity, statMod.damageType);
                }
                else if (statMod.changeValue < 0)
                {
                    Color color = Color.red;
                    CombatEvents.current.DamageDealt(((int)statMod.changeValue), entity, statMod.damageType);
                }

                entity.updateHealthBar();
                if (statValue <= 0)
                {
                    CombatEvents.current.UnitDeath(entity);
                }
            }
        }

        statMods.RemoveAll(x => x.duration == 0);

        if (statMods.Count == 0) { isModified = false; }
        else { isModified = true; }
    }
}

public class StatModifier
{
    public StatsList attributeName;
    public float value;
    public DamageTypes damageType;
    public float duration;
    public Operation op;
    public bool isActive;
    public string statModName;
    [HideInInspector] public float changeValue;

    public StatModifier(StatsList attributeName, float value, float duration, Operation op, string statModName, DamageTypes damageType)
    {
        this.attributeName = attributeName;
        this.value = value;
        this.duration = duration;
        this.op = op;
        this.statModName = statModName;
        this.damageType = damageType;
        isActive = true;
    }
}
