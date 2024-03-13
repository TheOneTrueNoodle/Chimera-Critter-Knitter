using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class UnitHandler
{
    public void UnitSufferAbility(Entity target, AbilityData ability, int damage)
    {
        if (ability.damageType != DamageTypes.Healing) { UnitSufferDamage(target, damage); }
        else if (damage > 0 && ability.damageType == DamageTypes.Healing) { UnitHeals(target, damage); }
    }

    public void UnitSufferEffect(Entity target, ScriptableEffect effect)
    {
        if (effect)
        {
            var statToEffect = target.activeStatsDir[System.Enum.GetName(typeof(StatsList), effect.statKey)];
            if (statToEffect.statMods.FindIndex(x => x.statModName == effect.name) != -1)
            {
                int modIndex = statToEffect.statMods.FindIndex(x => x.statModName == effect.name);
                statToEffect.statMods[modIndex] = new StatModifier(effect.statKey, effect.value, effect.duration, effect.op, effect.name, effect.damageType);
            }
            else
                statToEffect.statMods.Add(new StatModifier(effect.statKey, effect.value, effect.duration, effect.op, effect.name, effect.damageType));

            statToEffect.ApplyStatMod(target);
        }
    }

    public void UnitSufferDamage(Entity target, int damage)
    {
        Debug.Log("Taken " + damage + " damage");
        target.activeStatsDir["MaxHP"].statValue -= damage;

        target.BloodSplash();
        if(!target.CharacterData.takeDamageSFX.IsNull) { AudioManager.instance.PlayOneShot(target.CharacterData.takeDamageSFX, target.transform.position); } else { Debug.LogError("NO TAKE DAMAGE SOUND ASSIGNED"); }

        if (target.activeStatsDir["MaxHP"].statValue <= 0)
        {
            Debug.Log("Die loser");
            UnitDeath(target);
        }
    }

    public void UnitHeals(Entity target, int value)
    {
        target.activeStatsDir["MaxHP"].statValue += value;
        if (target.activeStatsDir["MaxHP"].statValue > (int)target.activeStatsDir["MaxHP"].baseStatValue)
        {
            target.activeStatsDir["MaxHP"].statValue = (int)target.activeStatsDir["MaxHP"].baseStatValue;
        }
    }

    public void UnitDeath(Entity target)
    {
        if (target.activeTile != null)
        {
            target.activeTile.isBlocked = false;
            target.activeTile.activeCharacter = null;
        }

        target.isDead = true;
        target.Die();
        CombatEvents.current.UnitDeath(target);
    }
}
