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

    public void StartCombat(List<Entity> AllUnits, List<CombatRoundEventData> RoundEvents)
    {
        inCombat = true;
        turnOrder = AllUnits;
        turnOrder = turnOrder.OrderByDescending(i => i.activeStatsDir["Speed"].statValue).ToList();

        activeTurnOrder = new List<Entity>();
        foreach(Entity entity in turnOrder) 
        { 
            activeTurnOrder.Add(entity); 
        }
        roundNumber = 0;

        roundEvents = new List<CombatRoundEventData>();
        roundEvents = RoundEvents;

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
    }

    public void StartFirstTurn()
    {
        CombatEvents.current.TurnOrderDisplay(activeTurnOrder);
        CombatEvents.current.NewTurn(activeTurnOrder.First());
        CombatEvents.current.AddLog(activeTurnOrder.First().CharacterData.Name + " begins their turn!");

        if (!activeTurnOrder.First().CharacterData.newTurnSFX.IsNull)
        {
            AudioManager.instance.PlayOneShot(activeTurnOrder.First().CharacterData.newTurnSFX, activeTurnOrder.First().transform.position);
        }
        else { Debug.LogError("NO NEW TURN AUDIO ASSIGNED"); }
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

        CombatEvents.current.AddLog(new string(lastTurn.CharacterData.Name + " has ended their turn!"));

        if (activeTurnOrder.Count == 0)
        {
            //NEW ROUND
            NewRound();
        }
        else
        {
            CombatEvents.current.NewTurn(activeTurnOrder.First());
            CombatEvents.current.AddLog(activeTurnOrder.First().CharacterData.Name + " begins their turn!");

            if (!activeTurnOrder.First().CharacterData.newTurnSFX.IsNull)
            {
                AudioManager.instance.PlayOneShot(activeTurnOrder.First().CharacterData.newTurnSFX, activeTurnOrder.First().transform.position);
            }
            else { Debug.LogError("NO NEW TURN AUDIO ASSIGNED"); }
        }
    }

    public void AddUnitToTurnOrder(Entity entity)
    {
        turnOrder.Add(entity);
        turnOrder = turnOrder.OrderByDescending(i => i.activeStatsDir["Speed"].statValue).ToList();
    }

    public void UnitDeath(Entity target)
    {
        turnOrder.Remove(target);
        activeTurnOrder.Remove(target);
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

        CombatEvents.current.TurnOrderDisplay(activeTurnOrder);
        CombatEvents.current.NewTurn(activeTurnOrder.First());
        CombatEvents.current.AddLog(activeTurnOrder.First().CharacterData.Name + " begins their turn!");

        if (!activeTurnOrder.First().CharacterData.newTurnSFX.IsNull)
        {
            AudioManager.instance.PlayOneShot(activeTurnOrder.First().CharacterData.newTurnSFX, activeTurnOrder.First().transform.position);
        }
        else { Debug.LogError("NO NEW TURN AUDIO ASSIGNED"); }
    }
}
