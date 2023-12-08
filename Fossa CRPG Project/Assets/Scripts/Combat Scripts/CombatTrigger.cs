using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CombatTrigger : MonoBehaviour
{
    public string CombatName;
    public EventReference BattleTheme;
    public List<CombatAIController> EnemiesInThisCombat;
    public List<CombatAIController> OtherEntitiesInThisCombat;
    public List<CombatRoundEventData> RoundEvents;

    public void Call()
    {
        CombatEvents.current.StartCombat(EnemiesInThisCombat, OtherEntitiesInThisCombat, RoundEvents, BattleTheme);
    }
}
