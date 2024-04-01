using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class ActionHandler
{
    public int Attack(Entity attacker, Entity target)
    {
        bool AttackCrits = CalculateCrit(attacker);
        bool AttackHits = CalculateHitChance(attacker, target);
        int DamageDealt = CalculateDamage(attacker, target, AttackCrits);

        if (AttackCrits)
        {
            return DamageDealt;
        }
        else if (AttackHits)
        {
            return DamageDealt;
        }
        else
        {
            return 0;
        }
    }

    public int Ability(Entity attacker, Entity target, AbilityData ability)
    {
        return CalculateMagicDamage(attacker, target, ability);
    }

    #region Attack Calculations
    private bool CalculateCrit(Entity attacker)
    {
        float critChance = (attacker.activeStatsDir["CriticalChance"].statValue * 0.1f);
        float critRole = Random.Range(0, 100);

        if (critRole <= critChance) { return true; }

        return false;
    }
    private bool CalculateHitChance(Entity attacker, Entity target)
    {
        //FORMULA Attacker Accuracy / Attacker Accuracy - (1 - Defender Evasion / Attacker Accuracy)
        float accuracy = attacker.activeStatsDir["Accuracy"].statValue;
        float evasion = target.activeStatsDir["Dodge"].statValue;

        float hitChance = 1 - (evasion / (3 * accuracy));
        if (hitChance < 0.05f) { hitChance = 0.05f; }
        hitChance *= 100;

        float hitRole = Random.Range(0, 100);
        if (hitRole <= hitChance)
        {
            //Attack hits
            return true; 
        }
        return false;
    }
    private int CalculateDamage(Entity attacker, Entity target, bool crits)
    {
        int rawDamage = (int)attacker.activeStatsDir["Attack"].statValue;
        if (attacker.CharacterData.Weapon != null) { rawDamage += attacker.CharacterData.Weapon.weaponDamage; }

        if (crits)
        {
            rawDamage *= 2;
        }

        int takenDamage;
        if ((int)attacker.AttackDamageType < 3)
        {
            takenDamage = (int)(rawDamage * 100 / (100 + target.activeStatsDir["Defence"].statValue));
        }
        else
        {
            takenDamage = (int)(rawDamage * 100 / (100 + target.activeStatsDir["MagicDefence"].statValue));
        }

        takenDamage = (int)(takenDamage * (target.isDefending == true ? 0.5 : 1));
        if (target.Resistances.Contains(attacker.AttackDamageType)) { takenDamage /= 2; }
        if (target.Weaknesses.Contains(attacker.AttackDamageType)) { takenDamage *= 2; }

        if (takenDamage < 0) { takenDamage = 0; }

        return takenDamage;
    }
    #endregion
    #region Ability Calculations
    public List<Entity> CalculateAbilityTargets(Entity caster, AbilityData.AbilityTypes abilityType, List<Entity> targets)
    {
        List<Entity> abilityTargets = new List<Entity>();

        if (abilityType == AbilityData.AbilityTypes.Enemy)
        {
            foreach (Entity entity in targets)
            {
                if (entity.TeamID != caster.TeamID) { abilityTargets.Add(entity); }
            }
        }
        else if (abilityType == AbilityData.AbilityTypes.Ally)
        {
            foreach (Entity entity in targets)
            {
                if (entity.TeamID == caster.TeamID) { abilityTargets.Add(entity); }
            }
        }
        else if (abilityType == AbilityData.AbilityTypes.All)
        {
            foreach (Entity entity in targets) { abilityTargets.Add(entity); }
        }

        return abilityTargets;
    }

    public int CalculateMagicDamage(Entity attacker, Entity target, AbilityData ability)
    {
        int rawDamage = (int)(ability.value * (100 + attacker.activeStatsDir["MagicAttack"].statValue) / 100);

        int takenDamage;
        if ((int)attacker.AttackDamageType < 3)
        {
            takenDamage = (int)(rawDamage * 100 / (100 + target.activeStatsDir["Defence"].statValue));
        }
        else
        {
            takenDamage = (int)(rawDamage * 100 / (100 + target.activeStatsDir["MagicDefence"].statValue));
        }

        takenDamage = (int)(takenDamage * (target.isDefending == true ? 0.5 : 1));
        if (target.Resistances.Contains(attacker.AttackDamageType)) { takenDamage /= 2; }
        if (target.Weaknesses.Contains(attacker.AttackDamageType)) { takenDamage *= 2; }

        if (takenDamage < 0) { takenDamage = 0; }

        return takenDamage;
    }
    #endregion
}
