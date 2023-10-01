using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "New_Unit_Data", menuName = "Combat/Units/New Unit Data", order = 0)]
public class UnitData : ScriptableObject
{
    [Header("General Information")]

    public string Name;
    public Sprite portrait;

    [Header("Leveling Information")]
    public LevelConfig levelConfig;
    public float level_modifier;

    [Header("Stat Information")]
    public Dictionary<string, Stat> statsDir;
    public UnitStats characterClass;

    [Header("Ability Information")]
    public List<AbilityData> Abilities;

    [Header("Equipment Information")]
    public List<WeaponType> equippableWeapons;

    public ScriptableWeapon Weapon;
    public ScriptableArmour Armour;
    public ScriptableAccessory Accessory1;
    public ScriptableAccessory Accessory2;

    [Header("Loot Tables")]
    //A LIST OF DROPPABLE ITEMS WITH WEIGHTS FOR DROP CHANCE
    public int maxExpDrop;
    public int minExpDrop;

    public void SetDictionaryStats(int level)
    {
        if (statsDir == null)
            statsDir = new Dictionary<string, Stat>();

        var fields = typeof(UnitStats).GetFields();
        for (int i = 1; i < fields.Length; i++)
        {
            float value = (float)fields[i].GetValue(characterClass);

            //If already declared, just update the stat
            if (statsDir.ContainsKey(fields[i].Name))
            {
                statsDir[fields[i].Name].updateStat(Mathf.Round(value * (level * level_modifier)));
            }
            else
            {
                statsDir.Add(fields[i].Name, new Stat(fields[i].Name, Mathf.Round(value * (level * level_modifier))));
            }
        }
    }

    public List<AbilityData> SetAbilities(List<AbilityData> additionalAbilities)
    {
        List<AbilityData> currentAbilities = new List<AbilityData>();
        if(additionalAbilities != null)
        {
            foreach (AbilityData ability in additionalAbilities)
            {
                currentAbilities.Add(ability);
            }
        }
        if(Abilities != null)
        {
            foreach (AbilityData ability in Abilities)
            {
                currentAbilities.Add(ability);
            }
        }

        return currentAbilities;
    }

    public void SetEquipment()
    {
        if (Weapon != null)
        {
            if (!equippableWeapons.Contains(Weapon.weaponType))
            {
                Weapon = null;
            }
            else if (Weapon.characterSpecific)
            {
                if (Name != Weapon.characterName) { Weapon = null; }
            }

        }
        if (Armour != null && Armour.characterSpecific)
        {
            if (Name != Armour.characterName) { Armour = null; }
        }

        if (Accessory1 != null && Accessory1.characterSpecific)
        {
            if (Name != Accessory1.characterName) { Accessory1 = null; }
        }

        if (Accessory2 != null && Accessory2.characterSpecific)
        {
            if (Name != Accessory2.characterName) { Accessory2 = null; }
        }
    }

    public List<EquipmentStatChanges> SetEquipmentStatChanges()
    {
        List<EquipmentStatChanges> returnStatChanges = new List<EquipmentStatChanges>();
        if (Weapon != null)
        {
            foreach (EquipmentStatChanges statChange in Weapon.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }
        if (Armour != null)
        {
            foreach (EquipmentStatChanges statChange in Armour.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }
        if (Accessory1 != null)
        {
            foreach (EquipmentStatChanges statChange in Accessory1.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }
        if (Accessory2 != null)
        {
            foreach (EquipmentStatChanges statChange in Accessory2.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }

        return returnStatChanges;
    }

    public List<ScriptableEffect> SetStartingEffects()
    {
        List<ScriptableEffect> returnEffects = new List<ScriptableEffect>();
        if (Weapon != null)
        {
            foreach (ScriptableEffect effect in Weapon.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }
        if (Armour != null)
        {
            foreach (ScriptableEffect effect in Armour.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }
        if (Accessory1 != null)
        {
            foreach (ScriptableEffect effect in Accessory1.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }
        if (Accessory2 != null)
        {
            foreach (ScriptableEffect effect in Accessory2.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }

        return returnEffects;
    }

    public List<DamageTypes> SetResistances()
    {
        List<DamageTypes> resistances = new List<DamageTypes>();
        if (Armour != null)
        {
            foreach (DamageTypes damageType in Armour.Resistances)
            {
                if (!resistances.Contains(damageType)) { resistances.Add(damageType); }
            }
        }
        if (Accessory1 != null)
        {
            foreach (DamageTypes damageType in Accessory1.Resistances)
            {
                if (!resistances.Contains(damageType)) { resistances.Add(damageType); }
            }
        }
        if (Accessory2 != null)
        {
            foreach (DamageTypes damageType in Accessory2.Resistances)
            {
                if (!resistances.Contains(damageType)) { resistances.Add(damageType); }
            }
        }

        return resistances;
    }
    public List<DamageTypes> SetWeaknesses()
    {
        List<DamageTypes> weaknesses = new List<DamageTypes>();
        if (Armour != null)
        {
            foreach (DamageTypes damageType in Armour.Weaknesses)
            {
                if (!weaknesses.Contains(damageType)) { weaknesses.Add(damageType); }
            }
        }
        if (Accessory1 != null)
        {
            foreach (DamageTypes damageType in Accessory1.Weaknesses)
            {
                if (!weaknesses.Contains(damageType)) { weaknesses.Add(damageType); }
            }
        }
        if (Accessory2 != null)
        {
            foreach (DamageTypes damageType in Accessory2.Weaknesses)
            {
                if (!weaknesses.Contains(damageType)) { weaknesses.Add(damageType); }
            }
        }

        return weaknesses;
    }
}
