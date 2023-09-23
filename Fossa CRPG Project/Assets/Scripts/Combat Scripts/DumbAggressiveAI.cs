using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAggressiveAI : BaseAI
{
    public override Scenario Attack(OverlayTile tile, CombatAIController entity)
    {
        var targetCharacter = entity.FindClosestCharacter(tile);

        if (targetCharacter)
        {
            var closestDistance = entity.pathFinder.GetManhattenDistance(tile, targetCharacter.activeTile);

            //Check if the closest character is in attack range and make sure we're not on the characters tile. 
            if (closestDistance <= entity.WeaponRange && tile != targetCharacter)
            {
                var targetTile = entity.GetClosestNeighbour(targetCharacter.activeTile);

                //calculate senarioValue;
                var scenarioValue = 0;
                scenarioValue += (int)entity.activeStatsDir["Attack"].statValue + entity.CharacterData.characterClass.MovementSpeed - closestDistance;

                if (entity.CharacterData.Weapon != null) { scenarioValue += entity.CharacterData.Weapon.weaponDamage; }

                return new Scenario(scenarioValue, null, targetCharacter.activeTile, tile, true);
            }
        }

        return new Scenario();
    }
}

