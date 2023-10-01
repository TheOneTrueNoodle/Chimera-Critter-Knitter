using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public List<CreatureBodyPartSlot> bodyParts;
    [HideInInspector] public Entity thisEntity;

    private void Start()
    {
        var foundParts = GetComponentsInChildren<CreatureBodyPartSlot>();

        foreach(var part in foundParts)
        {
            bodyParts.Add(part);
        }
    }

    public void ApplyPartEffects()
    {
        List<EquipmentStatChanges> statChanges = new List<EquipmentStatChanges>();
        List<ScriptableEffect> effects = new List<ScriptableEffect>();
        List<AbilityData> abilities = new List<AbilityData>();

        foreach(var part in bodyParts)
        {
            if(part.activePart != null)
            {
                //Apply that parts effects!
                if (part.activePart.mutations != null)
                {
                    foreach(ScriptableEffect newEffect in part.activePart.mutations)
                    {
                        effects.Add(newEffect);
                    }
                }

                if (part.activePart.statMutations != null)
                {
                    foreach (EquipmentStatChanges statChange in part.activePart.statMutations)
                    {
                        statChanges.Add(statChange);
                    }
                }

                if(part.activePart.abilities != null)
                {
                    foreach(AbilityData ability in part.activePart.abilities)
                    {
                        abilities.Add(ability);
                    }
                }
            }
        }

        CombatEvents.current.UnitStartingEffects(thisEntity, statChanges, effects);
        //ADD ABILITY SETUP
    }
}
