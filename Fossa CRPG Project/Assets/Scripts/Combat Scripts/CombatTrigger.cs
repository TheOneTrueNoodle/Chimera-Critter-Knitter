using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public List<CombatAIController> EnemiesInThisCombat;
    public List<CombatAIController> OtherEntitiesInThisCombat;
    //A list of the combat round events that will trigger

    public void Call()
    {
        CombatEvents.current.StartCombat(EnemiesInThisCombat, OtherEntitiesInThisCombat);
    }
}
