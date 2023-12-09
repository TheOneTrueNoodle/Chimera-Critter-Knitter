using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CombatEvents : MonoBehaviour
{
    public static CombatEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<List<CombatAIController>, List<CombatAIController>, List<CombatRoundEventData>, float> onStartCombat;
    public void StartCombat(List<CombatAIController> enemies, List<CombatAIController> others, List<CombatRoundEventData> RoundEvents, float BattleTheme)
    {
        if (onStartCombat != null)
        {
            onStartCombat(enemies, others, RoundEvents, BattleTheme);
        }
    }

    public event Action onStartCombatSetup;
    public void StartCombatSetup()
    {
        if(onStartCombatSetup != null)
        {
            onStartCombatSetup();
        }
    }

    public event Action<List<Entity>> onTurnOrderDisplay;
    public void TurnOrderDisplay(List<Entity> allUnits)
    {
        if (onTurnOrderDisplay != null)
        {
            onTurnOrderDisplay(allUnits);
        }
    }

    public event Action onEndCombat;
    public void EndCombat()
    {
        if (onEndCombat != null)
        {
            onEndCombat();
        }
    }

    public event Action<Entity> onNewTurn;
    public void NewTurn(Entity activeChar)
    {
        if (onNewTurn != null)
        {
            onNewTurn(activeChar);
        }
    }

    public event Action onTurnEnd;
    public void TurnEnd()
    {
        if (onTurnEnd != null)
        {
            onTurnEnd();
        }
    }

    public event Action<int, AbilityData> onSetCursorMode;
    public void SetCursorMode(int mode, AbilityData abilityData)
    {
        if (onSetCursorMode != null)
        {
            onSetCursorMode(mode, abilityData);
        }
    }

    public event Action onActionComplete;
    public void ActionComplete()
    {
        if (onActionComplete != null)
        {
            onActionComplete();
        }
    }

    public event Action<GameObject> onToggleFollowCamera;
    public void ToggleFollowCamera(GameObject obj)
    {
        if (onToggleFollowCamera != null)
        {
            onToggleFollowCamera(obj);
        }
    }

    public event Action<int, Entity, Combat.DamageTypes> onDamageDealt;
    public void DamageDealt(int damage, Entity target, Combat.DamageTypes damageType)
    {
        if (onDamageDealt != null)
        {
            onDamageDealt(damage, target, damageType);
        }
    }

    public event Action<Entity> onUnitDeath;
    public void UnitDeath(Entity unit)
    {
        if (onUnitDeath != null)
        {
            onUnitDeath(unit);
        }
    }

    public event Action<Entity, Entity> onAttackAttempt;
    public void AttackAttempt(Entity attacker, Entity target)
    {
        if (onAttackAttempt != null)
        {
            onAttackAttempt(attacker, target);
        }
    }
    public event Action<Entity, List<Entity>, AbilityData, Vector3> onAbilityAttempt;
    public void AbilityAttempt(Entity attacker, List<Entity> targets, AbilityData ability, Vector3 AbilityCenter)
    {
        if (onAbilityAttempt != null)
        {
            onAbilityAttempt(attacker, targets, ability, AbilityCenter);
        }
    }
    public event Action<Entity, List<OverlayTile>> onMoveAttempt;
    public void MoveAttempt(Entity entity, List<OverlayTile> path)
    {
        if (onMoveAttempt != null)
        {
            onMoveAttempt(entity, path);
        }
    }

    //TILE STUFF
    public event Action<OverlayTile> onTileClicked;
    public void TileClicked(OverlayTile overlayTile)
    {
        if (onTileClicked != null)
        {
            onTileClicked(overlayTile);
        }
    }

    public event Action<Entity, Color, List<OverlayTile>, bool> onTileColor;
    public void TileColor(Entity entity, Color color, List<OverlayTile> overlayTiles, bool hideOccupiedTile)
    {
        if (onTileColor != null)
        {
            onTileColor(entity, color, overlayTiles, hideOccupiedTile);
        }
    }

    public event Action<List<OverlayTile>> onTileClearSpecific;
    public void TileClearSpecific(List<OverlayTile> overlayTiles)
    {
        if (onTileClearSpecific != null)
        {
            onTileClearSpecific(overlayTiles);
        }
    }

    public event Action<Entity, OverlayTile> onPositionEntity;
    public void TilePositionEntity(Entity entity, OverlayTile overlayTile)
    {
        if (onPositionEntity != null)
        {
            onPositionEntity(entity, overlayTile);
        }
    }

    public event Action<Entity, List<EquipmentStatChanges>, List<ScriptableEffect>> onUnitStartingEffects;
    public void UnitStartingEffects(Entity entity, List<EquipmentStatChanges> equipmentStatChanges, List<ScriptableEffect> additionalEffects)
    {
        if (onUnitStartingEffects != null)
        {
            onUnitStartingEffects(entity, equipmentStatChanges, additionalEffects);
        }
    }

    public event Action<Entity, List<AbilityData>> onUnitStartingAbilities;
    public void UnitStartingAbilities(Entity entity, List<AbilityData> abilities)
    {
        if (onUnitStartingAbilities != null)
        {
            onUnitStartingAbilities(entity, abilities);
        }
    }

    public event Action<Entity> onAddUnitToCombat;
    public void AddUnitToCombat(Entity entity)
    {
        if(onAddUnitToCombat != null)
        {
            onAddUnitToCombat(entity);
        }
    }

    public event Action<OverlayTile> onGetSelectedTile;
    public void GetSelectedTile(OverlayTile overlayTile)
    {
        if (onGetSelectedTile != null)
        {
            onGetSelectedTile(overlayTile);
        }
    }
}
