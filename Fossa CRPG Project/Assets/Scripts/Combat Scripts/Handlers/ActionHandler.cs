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
        int DamageDealt = CalculateDamage(attacker, AttackCrits);

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
        return CalculateMagicDamage(attacker, ability);
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
        float accuracy = Random.Range(0.5f, 3) * attacker.activeStatsDir["Accuracy"].statValue;

        float dodgeChance = target.activeStatsDir["Dodge"].statValue * 0.1f;
        float dodgeRole = Random.Range(0, accuracy);

        if (dodgeRole > dodgeChance) { return true; }

        return false;
    }
    private int CalculateDamage(Entity attacker, bool crits)
    {
        int rawDamage = (int)attacker.activeStatsDir["Attack"].statValue;
        if (attacker.CharacterData.Weapon != null) { rawDamage += attacker.CharacterData.Weapon.weaponDamage; }

        if (crits)
        {
            rawDamage *= 2;
        }

        return rawDamage;
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

    public int CalculateMagicDamage(Entity attacker, AbilityData ability)
    {
        int rawDamage = (int)(ability.value * (100 + attacker.activeStatsDir["MagicAttack"].statValue) / 100);

        return rawDamage;
    }
    #endregion
}
