using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public string CombatName;
    public List<CombatAIController> EnemiesInThisCombat;
    public List<CombatAIController> OtherEntitiesInThisCombat;
    public List<CombatRoundEventData> RoundEvents;

    public void Call()
    {
        CombatEvents.current.StartCombat(EnemiesInThisCombat, OtherEntitiesInThisCombat, RoundEvents);
        CombatEvents.current.StartCombatSetup();
    }
}
