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
    public List<Entity> enemyTeam;
    public List<Entity> otherTeam;

    private void Start()
    {
        CombatEvents.current.onUnitDeath += UnitDeath;
        CombatEvents.current.onUnitStartingEffects += UnitStartingEffects;
        CombatEvents.current.onMoveAttempt += MoveAttempt;
        CombatEvents.current.onAttackAttempt += AttackAttempt;
        CombatEvents.current.onAbilityAttempt += AbilityAttempt;
        CombatEvents.current.onDamageDealt += DisplayDamageText;

        CombatEvents.current.onTileColor += TileColor;
        CombatEvents.current.onTileClearSpecific += TileClearSpecific;
        CombatEvents.current.onPositionEntity += TilePositionEntity;

        CombatEvents.current.onTurnEnd += TurnEnd;
    }

    public void StartCombat()
    {
        turnHandler = new TurnHandler();
        moveHandler = new MoveHandler();
        actionHandler = new ActionHandler();
        unitHandler = new UnitHandler();
        tileHandler = new TileHandler();

        turnHandler.StartCombat(FindAllUnits());
    }

    public void TurnEnd()
    {
        tileHandler.ClearTiles();
        StartCoroutine(turnHandler.DelayedTurnEnd());
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
        if (damage > 0)
        {
            unitHandler.UnitSufferDamage(target, damage);
        }

        DisplayDamageText(damage, target, attacker.AttackDamageType);
        CombatEvents.current.TurnEnd();
    }

    public void AbilityAttempt(Entity attacker, List<Entity> targets, AbilityData ability)
    {
        List<Entity> affectedTargets = actionHandler.CalculateAbilityTargets(attacker, ability.abilityType, targets);

        foreach (Entity target in affectedTargets)
        {
            int damage = actionHandler.Ability(attacker, target, ability);
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

            if (physicalDamage > 0)
            {
                unitHandler.UnitSufferDamage(target, physicalDamage);
                DisplayDamageText(physicalDamage, target, attacker.AttackDamageType);
            }

            unitHandler.UnitSufferAbility(target, ability, damage);
            DisplayDamageText(damage, target, ability.damageType);
        }

        CombatEvents.current.TurnEnd();
    }

    public void UnitDeath(Entity target)
    {
        unitHandler.UnitDeath(target);
        turnHandler.UnitDeath(target);
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
    private List<Entity> FindAllUnits()
    {
        var ReturnUnits = new List<Entity>();

        var UnitsUnsorted = FindObjectsOfType<Entity>();
        playerTeam = new List<Entity>();
        enemyTeam = new List<Entity>();
        otherTeam = new List<Entity>();

        List<CombatAIController> ai = new List<CombatAIController>(FindObjectsOfType<CombatAIController>());
        foreach (CombatAIController enemy in ai)
        {
            enemy.InitializeCombat(playerTeam);
        }

        foreach (var item in UnitsUnsorted)
        {
            item.Initialize();
            ReturnUnits.Add(item);

            if (item.TeamID == 0)
            {
                playerTeam.Add(item);
            }
            else if (item.TeamID == 1)
            {
                enemyTeam.Add(item);
            }
            else
            {
                otherTeam.Add(item);
            }
        }

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
    #endregion
}