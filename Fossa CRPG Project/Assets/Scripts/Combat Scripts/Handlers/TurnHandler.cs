using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnHandler
{
    [SerializeField] private List<Entity> turnOrder;

    public void StartCombat(List<Entity> AllUnits)
    {
        turnOrder = AllUnits;
        turnOrder = turnOrder.OrderByDescending(i => i.activeStatsDir["Speed"].statValue).ToList();

        CombatEvents.current.StartCombat();
        CombatEvents.current.NewTurn(turnOrder.First());
    }

    public void EndCombat()
    {
        turnOrder.Clear();
    }

    public void nextTurn()
    {
        var lastTurn = turnOrder.First();
        turnOrder.RemoveAt(0);
        turnOrder.Add(lastTurn);

        CombatEvents.current.NewTurn(turnOrder.First());
    }

    public void AddUnitToTurnOrder(Entity entity)
    {
        turnOrder.Add(entity);
    }

    public IEnumerator DelayedTurnEnd()
    {
        yield return new WaitForSeconds(1);
        nextTurn();
    }

    public void UnitDeath(Entity target)
    {
        turnOrder.Remove(target);
    }
}
