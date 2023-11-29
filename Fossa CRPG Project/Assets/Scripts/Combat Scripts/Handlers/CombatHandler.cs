using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class CombatHandler : MonoBehaviour
{
    [HideInInspector] public TurnHandler turnHandler;
    [HideInInspector] public MoveHandler moveHandler;
    [HideInInspector] public ActionHandler actionHandler;
    [HideInInspector] public UnitHandler unitHandler;
    [HideInInspector] public TileHandler tileHandler;

    public GameObject damageTextPrefab;

    //Unit Teams
    public List<Entity> playerTeam;
    public List<CombatAIController> enemyTeam;
    public List<CombatAIController> otherTeam;
    public List<CombatObstacle> obstacles;

    //Dropped Loot after combat
    //List of dropped items
    public int totalDroppedExp;

    private void Start()
    {
        CombatEvents.current.onStartCombat += StartCombat;

        CombatEvents.current.onUnitDeath += UnitDeath;
        CombatEvents.current.onUnitStartingEffects += UnitStartingEffects;
        CombatEvents.current.onUnitStartingAbilities += UnitStartingAbilities;
        CombatEvents.current.onMoveAttempt += MoveAttempt;
        CombatEvents.current.onAttackAttempt += AttackAttempt;
        CombatEvents.current.onAbilityAttempt += AbilityAttempt;
        CombatEvents.current.onDamageDealt += DisplayDamageText;

        CombatEvents.current.onTileColor += TileColor;
        CombatEvents.current.onTileClearSpecific += TileClearSpecific;
        CombatEvents.current.onPositionEntity += TilePositionEntity;

        CombatEvents.current.onTurnEnd += TurnEnd;
    }

    public void StartCombat(List<CombatAIController> enemies, List<CombatAIController> others, List<CombatRoundEventData> RoundEvents)
    {
        if (turnHandler == null) { turnHandler = new TurnHandler(); }
        if (moveHandler == null) { moveHandler = new MoveHandler(); }
        if (actionHandler == null){ actionHandler = new ActionHandler(); }
        if (unitHandler == null) { unitHandler = new UnitHandler(); }
        if (tileHandler == null) { tileHandler = new TileHandler(); }

        var activeUnits = FindAllActiveUnits(enemies, others);
        turnHandler.StartCombat(activeUnits, RoundEvents);

        totalDroppedExp = 0;
    }

    public void EndCombat()
    {
        Debug.Log("Combat over");
        Debug.Log("Player Number: " + playerTeam.Count);
        Debug.Log("EXP: " + totalDroppedExp);
        if (totalDroppedExp > 0)
        {
            totalDroppedExp /= playerTeam.Count;
            foreach (Entity entity in playerTeam)
            {
                entity.IncreaseEXP(totalDroppedExp);
            }
        }

        playerTeam.Clear();
        enemyTeam.Clear();
        otherTeam.Clear();

        turnHandler.EndCombat();
        CombatEvents.current.EndCombat();
    }

    public IEnumerator DelayedTurnEnd()
    {
        yield return new WaitForSeconds(0.5f);
        CombatEvents.current.TurnEnd();
    }

    public void TurnEnd()
    {
        tileHandler.ClearTiles();
        turnHandler.nextTurn();
    }

    #region Combat Interactions
    public void MoveAttempt(Entity entity, List<OverlayTile> path)
    {
        Debug.Log("Moving");
        StartCoroutine(moveHandler.MoveAlongPath(entity, path));
    }

    public void AttackAttempt(Entity attacker, Entity target)
    {
        int damage = actionHandler.Attack(attacker, target);
        if (damage > 0 && !target.isDead)
        {
            if(target.TryGetComponent(out CombatObstacle obstacle))
            {
                if (obstacle.Destructable)
                {
                    unitHandler.UnitSufferDamage(target, damage);
                    DisplayDamageText(damage, target, attacker.AttackDamageType);
                }
            }
            else
            {
                unitHandler.UnitSufferDamage(target, damage);
                DisplayDamageText(damage, target, attacker.AttackDamageType);
            }
        }

        StartCoroutine(DelayedTurnEnd());
    }

    public void AbilityAttempt(Entity attacker, List<Entity> targets, AbilityData ability)
    {
        List<Entity> affectedTargets = actionHandler.CalculateAbilityTargets(attacker, ability.abilityType, targets);
        Debug.Log(affectedTargets.Count);

        foreach (Entity target in affectedTargets)
        {
            int abilityDamage = actionHandler.Ability(attacker, target, ability);
            int physicalDamage = 0;

            if (ability.addMeleeAttack)
            {
                physicalDamage = actionHandler.Attack(attacker, target);
            }

            foreach (var effect in ability.effects)
            {
                if (effect.affectUser)
                    unitHandler.UnitSufferEffect(attacker, effect);
                else
                    unitHandler.UnitSufferEffect(target, effect);
            }

            if (physicalDamage > 0 && !target.isDead)
            {
                unitHandler.UnitSufferDamage(target, physicalDamage);
                DisplayDamageText(physicalDamage, target, attacker.AttackDamageType);
                Debug.Log("Displayed Phys Damage: " + physicalDamage);
            }
            if(abilityDamage > 0 && !target.isDead)
            {
                unitHandler.UnitSufferAbility(target, ability, abilityDamage);
                DisplayDamageText(abilityDamage, target, ability.damageType);
                Debug.Log("Displayed Abil Damage: " + abilityDamage);
            }
        }

        StartCoroutine(DelayedTurnEnd());
    }

    public void UnitDeath(Entity target)
    {
        turnHandler.UnitDeath(target);

        //Find dropped items
        totalDroppedExp += Random.Range(target.CharacterData.minExpDrop, target.CharacterData.maxExpDrop);
        if (target.TeamID == 1)
        {
            enemyTeam.Remove(target.GetComponent<CombatAIController>());
        }

        if (enemyTeam.Count <= 0)
        {
            EndCombat();
        }
    }

    public void DisplayDamageText(int damage, Entity target, DamageTypes damageType)
    {
        string dmgText = damage.ToString();
        if (damage <= 0) { dmgText = "MISS"; }
        GameObject dmgDisp = Instantiate(damageTextPrefab);
        dmgDisp.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2.5f, target.transform.position.z);

        dmgDisp.GetComponent<DamageText>().Setup(dmgText, damageType);
    }
    #endregion

    #region Tile Functions

    public void TileColor(Entity entity, Color color, List<OverlayTile> tiles, bool hideActiveTile)
    {
        tileHandler.ColorTiles(entity, color, tiles, hideActiveTile);
    }

    public void TileClearSpecific(List<OverlayTile> tiles)
    {
        tileHandler.ClearSpecificTiles(tiles);
    }

    public void TilePositionEntity(Entity entity, OverlayTile tile)
    {
        tileHandler.PositionCharacterOnTile(entity, tile);
    }

    #endregion

    #region Initialization
    private List<Entity> FindAllActiveUnits(List<CombatAIController> enemies, List<CombatAIController> others)
    {
        var ReturnUnits = new List<Entity>();

        var UnitsUnsorted = FindObjectsOfType<Entity>();
        playerTeam = new List<Entity>();
        enemyTeam = enemies;
        otherTeam = others;

        foreach (var item in UnitsUnsorted)
        {
            if (item.TeamID == 0)
            {
                Debug.Log("Found player");
                item.Initialize();
                ReturnUnits.Add(item);
                playerTeam.Add(item);
            }
            else if (item.TeamID == -1)
            {
                item.Initialize();
                obstacles.Add(item.GetComponent<CombatObstacle>());
            }
        }

        foreach (CombatAIController enemy in enemyTeam)
        {
            enemy.Initialize();
            enemy.InitializeCombat(playerTeam);
            ReturnUnits.Add(enemy);
        }

        /*
        foreach (CombatAIController other in otherTeam)
        {
            other.InitializeCombat(playerTeam);
            ReturnUnits.Add(other);
        }*/

        return ReturnUnits;
    }

    public void UnitStartingEffects(Entity entity, List<EquipmentStatChanges> equipmentStatChanges, List<ScriptableEffect> additionalEffects)
    {
        if (equipmentStatChanges != null)
        {
            for (int i = 0; i < equipmentStatChanges.Count; i++)
            {
                string statChangeName = equipmentStatChanges[i].statKey.ToString() + " " + i.ToString();

                var statToEffect = entity.activeStatsDir[System.Enum.GetName(typeof(StatsList), equipmentStatChanges[i].statKey)];
                statToEffect.statMods.Add(new StatModifier(equipmentStatChanges[i].statKey, equipmentStatChanges[i].value, -1, equipmentStatChanges[i].op, statChangeName, DamageTypes.Healing));

                statToEffect.ApplyStatMod(entity);
            }
        }
        if (additionalEffects != null)
        {
            foreach (ScriptableEffect effect in additionalEffects)
            {
                unitHandler.UnitSufferEffect(entity, effect);
            }
        }
    }

    public void UnitStartingAbilities(Entity entity, List<AbilityData> abilities)
    {
        entity.activeAbilities = entity.CharacterData.SetAbilities(abilities);
    }

    public void AddUnitToCombat(Entity entity)
    {
        turnHandler.AddUnitToTurnOrder(entity);

        if(entity.GetComponent<CombatAIController>())
        {
            entity.GetComponent<CombatAIController>().InitializeCombat(playerTeam);
        }
        entity.Initialize(); 
        if (entity.TeamID == 0)
        {
            playerTeam.Add(entity);
        }
        else if (entity.TeamID == 1)
        {
            
            enemyTeam.Add(entity.GetComponent<CombatAIController>());
        }
        else
        {
            otherTeam.Add(entity.GetComponent<CombatAIController>());
        }
    }
    #endregion
}