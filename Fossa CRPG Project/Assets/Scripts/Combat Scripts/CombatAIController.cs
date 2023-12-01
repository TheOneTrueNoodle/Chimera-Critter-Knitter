using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatAIController : Entity
{
    private BaseAI AI;

    public List<Entity> playerCharacters;

    private List<OverlayTile> path;
    private Scenario bestScenario;
    [HideInInspector] public ShapeParser shapeParser;
    [HideInInspector] public RangeFinder rangeFinder;
    [HideInInspector] public PathFinder pathFinder;

    public void InitializeCombat(List<Entity> playerCharacters)
    {
        this.playerCharacters = playerCharacters;
        AI = GetComponent<BaseAI>();
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
        shapeParser = new ShapeParser();
    }

    public override void StartTurn()
    {
        bestScenario = null;
        StartCoroutine(CalculateBestSenario());

        foreach (var item in activeStatsDir.Keys)
        {
            if (activeStatsDir[item].isModified)
            {
                activeStatsDir[item].UpdateStatMods(this);
                updateHealthBar();
                if (activeStatsDir[item].isModified)
                {
                    Debug.Log(item + " isModified = " + activeStatsDir[item].isModified);
                    Debug.Log("Duration " + activeStatsDir[item].statMods[0].duration);
                }
            }
        }

        updateHealthBar();
    }

    private IEnumerator CalculateBestSenario()
    {
        var tileInMovementRange = rangeFinder.GetMovementTilesInRange(this, CharacterData.characterClass.MovementSpeed, true, false);
        CombatEvents.current.TileColor(this, Color.white, tileInMovementRange, true);

        var scenario = new Scenario();
        foreach (var tile in tileInMovementRange)
        {
            if (!tile.isBlocked)
            {
                var tempSenario = CreateTileScenarioValue(tile);
                //ApplyTileEffectsToScenarioValue(tile, tempSenario);
                scenario = CompareScenarios(scenario, tempSenario);
                scenario = CheckIfScenarioValuesAreEqual(tileInMovementRange, scenario, tempSenario);
                scenario = CheckScenarioValueIfNoTarget(scenario, tile, tempSenario);
            }
        }


        if (scenario.positionTile)
        {
            ApplyBestScenario(scenario);
        }
        else
        {
            CombatEvents.current.TurnEnd();
        }

        yield return null;
    }
    private void ApplyBestScenario(Scenario scenario)
    {
        bestScenario = scenario;
        var currentTile = activeTile;
        path = pathFinder.FindPath(currentTile, bestScenario.positionTile, new List<OverlayTile>());

        //If it can attack but it doesn't need to move, attack
        if (path.Count == 0 && bestScenario.targetTile != null)
        {
            Attack();
        }
        else
        {
            //we're moving
            CombatEvents.current.MoveAttempt(this, path);
        }
    }

    #region Scenario Calculations
    private Scenario CreateTileScenarioValue(OverlayTile overlayTile)
    {
        //Basic Attack
        var attackSenario = AI.Attack(overlayTile, this);

        foreach (var abilityContainer in activeAbilities)
        {
            if (abilityContainer.costType == AbilityData.CostTypes.SP && activeStatsDir["MaxSP"].statValue >= abilityContainer.abilityCost && abilityContainer.cooldown >= abilityContainer.TurnsSinceUsed)
            {
                //Abilities
                var tempSenario = CreateAbilityScenario(abilityContainer, overlayTile);

                if (tempSenario.scenarioValue > attackSenario.scenarioValue)
                    attackSenario = tempSenario;
            }
            else if (abilityContainer.costType == AbilityData.CostTypes.HP && activeStatsDir["MaxHP"].statValue >= abilityContainer.abilityCost && abilityContainer.cooldown >= abilityContainer.TurnsSinceUsed)
            {
                //Abilities
                var tempSenario = CreateAbilityScenario(abilityContainer, overlayTile);

                if (tempSenario.scenarioValue > attackSenario.scenarioValue)
                    attackSenario = tempSenario;
            }
        }
        return attackSenario;
    }
    private Scenario CheckIfScenarioValuesAreEqual(List<OverlayTile> tileInMovementRange, Scenario scenario, Scenario tempScenario)
    {
        if (tempScenario.positionTile != null && tempScenario.scenarioValue == scenario.scenarioValue)
        {
            var tempSenarioPathCount = pathFinder.FindPath(activeTile, tempScenario.positionTile, tileInMovementRange).Count;
            var senarioPathCount = pathFinder.FindPath(activeTile, scenario.positionTile, tileInMovementRange).Count;

            if (tempSenarioPathCount < senarioPathCount)
                scenario = tempScenario;
        }

        return scenario;
    }
    private Scenario CheckScenarioValueIfNoTarget(Scenario scenario, OverlayTile tile, Scenario tempScenario)
    {
        if (tempScenario.positionTile == null && !scenario.targetTile)
        {
            var targetCharacter = FindClosestToDeathCharacter(tile);
            if (targetCharacter)
            {
                var targetTile = GetClosestNeighbour(targetCharacter.activeTile);

                if (targetCharacter && targetTile)
                {
                    var pathToCharacter = pathFinder.FindPath(tile, targetTile, new List<OverlayTile>());
                    var distance = pathToCharacter.Count;

                    var senarioValue = -distance - targetCharacter.activeStatsDir["MaxHP"].statValue;
                    if (distance >= WeaponRange)
                    {
                        /*
                        if (tile.tileData && tile.tileData.effect)
                        {
                            var tileEffectValue = GetEffectsScenarioValue(new List<ScriptableEffect>() { tile.tileData.effect }, new List<Entity>() { this });
                            senarioValue -= tileEffectValue;
                        }*/

                        if (tile.grid2DLocation != activeTile.grid2DLocation && tile.grid2DLocation != targetCharacter.activeTile.grid2DLocation && (senarioValue > scenario.scenarioValue || !scenario.positionTile))
                            scenario = new Scenario(senarioValue, null, null, tile, false);
                    }
                }
            }
        }

        return scenario;
    }
    private static Scenario CompareScenarios(Scenario scenario, Scenario tempScenario)
    {
        if ((tempScenario != null && tempScenario.scenarioValue > scenario.scenarioValue))
        {
            scenario = tempScenario;
        }

        return scenario;
    }
    private static int GetEffectsSenarioValue(List<ScriptableEffect> effectsContainer, List<Entity> entities)
    {
        var totalDamage = 0;
        int totalSenarioValue = 0;

        if (effectsContainer.Count > 0 && entities.Count > 0)
        {
            foreach (var effect in effectsContainer)
            {
                foreach (var entity in entities)
                {
                    if (effect.op == Combat.Operation.Minus)
                    {
                        totalDamage += Mathf.RoundToInt(effect.value * (effect.duration > 0 ? effect.duration : 1)); ;
                    }
                    else if (effect.op == Combat.Operation.MinusByPercentage)
                    {
                        var value = entity.activeStatsDir["MaxHP"].statValue / 100 * effect.value;
                        totalDamage += Mathf.RoundToInt(value * (effect.duration > 0 ? effect.duration : 1));
                    }


                    if (totalDamage >= entity.activeStatsDir["MaxHP"].statValue)
                        totalSenarioValue += 10000;
                    else
                        totalSenarioValue += totalDamage;
                }
            }
        }

        return totalSenarioValue;
    }

    private Scenario CreateAbilityScenario(AbilityData abilityContainer, OverlayTile position)
    {
        List<OverlayTile> startTiles = new List<OverlayTile>();
        startTiles.Add(position);

        var tilesInAbilityRange = rangeFinder.GetTilesInRange(startTiles, abilityContainer.range, false, true);
        var scenario = new Scenario();
        foreach (var tile in tilesInAbilityRange)
        {
            var abilityAffectedTiles = shapeParser.GetAbilityTileLocations(tile, abilityContainer.abilityShape, position.grid2DLocation);

            //How many players can the ability hit
            var inRangeCharacters = new List<Entity>();

            foreach (var tile2 in abilityAffectedTiles)
            {
                var targetCharacter = tile2.activeCharacter;
                if (targetCharacter != null && CheckAbilityTargets(abilityContainer.abilityType, targetCharacter))
                {
                    inRangeCharacters.Add(targetCharacter);
                }
                tile2.HideTile();
            }

            var totalAbilityDamage = GetEffectsSenarioValue(abilityContainer.effects, inRangeCharacters);

            if (inRangeCharacters.Count > 0)
            {
                var totalPlayerHealth = 0;
                var weakestPlayerHealth = int.MaxValue;
                var closestDistance = 0;
                var damageValue = 0;
                foreach (var player in inRangeCharacters)
                {
                    closestDistance = -1000;
                    totalPlayerHealth += (int)player.activeStatsDir["MaxHP"].statValue;

                    if (player.activeStatsDir["MaxHP"].statValue < weakestPlayerHealth)
                        weakestPlayerHealth = (int)player.activeStatsDir["MaxHP"].statValue;

                    var tempClosestDistance = pathFinder.GetManhattenDistance(position, player.activeTile);

                    if (tempClosestDistance > closestDistance)
                        closestDistance = tempClosestDistance;

                    totalAbilityDamage += abilityContainer.value;

                    if (abilityContainer.addMeleeAttack)
                    {
                        totalAbilityDamage += (int)activeStatsDir["Attack"].statValue;
                        if (CharacterData.Weapon != null) { totalAbilityDamage += CharacterData.Weapon.weaponDamage; }
                    }
                }

                damageValue += totalAbilityDamage;

                var tempSenarioValue = damageValue
                        + closestDistance
                        - weakestPlayerHealth;

                if (tempSenarioValue > scenario.scenarioValue)
                {
                    scenario = new Scenario(tempSenarioValue, abilityContainer, tile, position, false);
                }
            }
        }

        return scenario;
    }
    #endregion
    #region Additional Functions
    public Entity FindClosestCharacter(OverlayTile position)
    {
        Entity targetCharacter = null;

        var closestDistance = 1000;
        foreach (var player in playerCharacters)
        {
            if (player.isDead != true)
            {
                var currentDistance = pathFinder.GetManhattenDistance(position, player.activeTile);

                if (currentDistance <= closestDistance)
                {
                    closestDistance = currentDistance;
                    targetCharacter = player;
                }
            }
        }

        return targetCharacter;
    }
    private Entity FindClosestToDeathCharacter(OverlayTile position)
    {
        Entity targetCharacter = null;
        int lowestHealth = -1;
        var noCharacterInRange = true;
        foreach (var player in playerCharacters)
        {
            if (player.isDead != true && player.activeTile)
            {
                var currentDistance = pathFinder.GetManhattenDistance(position, player.activeTile);
                var currentHealth = (int)player.activeStatsDir["MaxHP"].statValue;

                if (currentDistance <= WeaponRange &&
                    ((lowestHealth == -1) || (currentHealth <= lowestHealth || noCharacterInRange)))
                {
                    lowestHealth = currentHealth;
                    targetCharacter = player;
                    noCharacterInRange = false;
                }
                else if (noCharacterInRange && ((lowestHealth == -1) || (currentHealth <= lowestHealth)))
                {
                    lowestHealth = currentHealth;
                    targetCharacter = player;
                }
            }
        }

        //can't travel to units tile so get the closest neighbour
        return targetCharacter;
    }

    public OverlayTile GetClosestNeighbour(OverlayTile targetCharacterTile)
    {
        var targetNeighbours = MapManager.Instance.GetNeighbourTiles(targetCharacterTile, new List<OverlayTile>(), false);
        var targetTile = targetNeighbours[0];
        var targetDistance = pathFinder.GetManhattenDistance(targetTile, activeTile);

        foreach (var item in targetNeighbours)
        {
            var distance = pathFinder.GetManhattenDistance(item, activeTile);

            if (distance < targetDistance)
            {
                targetTile = item;
                targetDistance = distance;
            }
        }

        return targetTile;
    }
    private bool CheckAbilityTargets(AbilityData.AbilityTypes abilityType, Entity characterTarget)
    {
        if (abilityType == AbilityData.AbilityTypes.Enemy)
        {
            return characterTarget.TeamID != TeamID;
        }
        else if (abilityType == AbilityData.AbilityTypes.Ally)
        {
            return characterTarget.TeamID == TeamID;
        }
        else
        {
            return false;
        }
    }
    private void PositionCharacterOnTile(OverlayTile tile)
    {
        if (activeTile != null && activeTile != tile)
        {
            activeTile.isBlocked = false;
            activeTile.activeCharacter = this;
        }
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y - 0.0001f, tile.transform.position.z);    
        activeTile = tile;
        activeTile.isBlocked = true;
        activeTile.activeCharacter = this;
    }
    public void CharacterMoved()
    {
        //Once a character has finished moving, check if a attack/ability is available and do it. Otherwise, end turn
        if (bestScenario != null && (bestScenario.targetTile != null || bestScenario.targetAbility != null))
            Attack();
        else
            CombatEvents.current.TurnEnd();
    }
    #endregion
    private void Attack()
    {
        //If we can attack, we attack, if we have an ability, cast the ability
        if (bestScenario.useAutoAttack && bestScenario.targetTile.activeCharacter)
            StartCoroutine(AttackTargettedCharacter(bestScenario.targetTile.activeCharacter));
        else if (bestScenario.targetAbility != null)
        {
            if (bestScenario.targetAbility.addMeleeAttack)
            {
                StartCoroutine(AttackTargettedCharacter(bestScenario.targetTile.activeCharacter));
            }
            StartCoroutine(CastAbility());
        }
    }
    private IEnumerator AttackTargettedCharacter(Entity targetedCharacter)
    {
        List<OverlayTile> tilesToColor = new List<OverlayTile>();
        tilesToColor.Add(targetedCharacter.activeTile);
        CombatEvents.current.TileColor(this, Color.red, tilesToColor, false);
        yield return new WaitForSeconds(0.5f);

        //As an example, damage is just the strenght stat. 
        CombatEvents.current.AttackAttempt(this, targetedCharacter);
    }
    private IEnumerator CastAbility()
    {
        var abilityAffectedTiles = shapeParser.GetAbilityTileLocations(bestScenario.targetTile, bestScenario.targetAbility.abilityShape, bestScenario.positionTile.grid2DLocation);

        Debug.Log(abilityAffectedTiles.Count);
        CombatEvents.current.TileColor(this, Color.blue, abilityAffectedTiles, false);
        yield return new WaitForSeconds(0.5f);

        var inRangeCharacters = new List<Entity>();

        foreach (var tile in abilityAffectedTiles)
        {
            var targetCharacter = tile.activeCharacter;

            if (targetCharacter != null && CheckAbilityTargets(bestScenario.targetAbility.abilityType, targetCharacter))
            {
                inRangeCharacters.Add(targetCharacter);
            }
            tile.HideTile();
        }

        if (bestScenario.targetAbility.requiresTarget && inRangeCharacters.Count > 0 || !bestScenario.targetAbility.requiresTarget)
        {
            if (bestScenario.targetAbility.costType == AbilityData.CostTypes.HP && bestScenario.targetAbility.abilityCost <= (int)activeStatsDir["MaxHP"].statValue)
            {
                Debug.Log("Casting " + bestScenario.targetAbility.Name);
                activeStatsDir["MaxHP"].statValue -= bestScenario.targetAbility.abilityCost;
                updateHealthBar();
            }
            else if (bestScenario.targetAbility.costType == AbilityData.CostTypes.SP && bestScenario.targetAbility.abilityCost <= (int)activeStatsDir["MaxSP"].statValue)
            {
                Debug.Log("Casting " + bestScenario.targetAbility.Name);
                activeStatsDir["MaxSP"].statValue -= bestScenario.targetAbility.abilityCost;
            }
            CombatEvents.current.AbilityAttempt(this, inRangeCharacters, bestScenario.targetAbility, bestScenario.targetTile.transform.position);
        }
    }
}

