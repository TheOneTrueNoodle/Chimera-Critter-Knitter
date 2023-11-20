using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnHandler
{
    public int roundNumber;

    [SerializeField] private List<Entity> turnOrder;
    [SerializeField] private List<Entity> activeTurnOrder;
    private List<CombatRoundEventData> roundEvents;

    bool inCombat;

    public void StartCombat(List<Entity> AllUnits)
    {
        inCombat = true;
        turnOrder = AllUnits;
        turnOrder = turnOrder.OrderByDescending(i => i.activeStatsDir["Speed"].statValue).ToList();

        activeTurnOrder = new List<Entity>();
        foreach(Entity entity in turnOrder) { activeTurnOrder.Add(entity); }
        roundNumber = 0;

        if(roundEvents != null)
        {
            foreach (CombatRoundEventData Event in roundEvents)
            {
                if (Event.RoundTrigger == roundNumber)
                {
                    //Trigger Event
                }
            }
        }

        CombatEvents.current.NewTurn(activeTurnOrder.First());
    }

    public void EndCombat()
    {
        inCombat = false;
        turnOrder.Clear();
        activeTurnOrder.Clear();
    }

    public void nextTurn()
    {
        if (!inCombat) { return; }
        var lastTurn = activeTurnOrder.First();
        activeTurnOrder.RemoveAt(0);

        if(activeTurnOrder.Count == 0)
        {
            //NEW ROUND
            NewRound();
        }
        else { CombatEvents.current.NewTurn(activeTurnOrder.First()); }
    }

    public void AddUnitToTurnOrder(Entity entity)
    {
        turnOrder.Add(entity);
        turnOrder = turnOrder.OrderByDescending(i => i.activeStatsDir["Speed"].statValue).ToList();
    }

    public IEnumerator DelayedTurnEnd()
    {
        yield return new WaitForSeconds(0.5f);
        nextTurn();
    }

    public void UnitDeath(Entity target)
    {
        turnOrder.Remove(target);
    }

    public void NewRound()
    {
        roundNumber++;
        activeTurnOrder.Clear();
        foreach (Entity entity in turnOrder) { activeTurnOrder.Add(entity); }

        if (roundEvents != null)
        {
            foreach (CombatRoundEventData Event in roundEvents)
            {
                if (Event.RoundTrigger == roundNumber)
                {
                    //Trigger Event
                }
            }
        }

        CombatEvents.current.NewTurn(activeTurnOrder.First());
    }
}
