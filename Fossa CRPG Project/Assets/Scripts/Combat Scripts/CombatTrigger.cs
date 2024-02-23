using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CombatTrigger : MonoBehaviour
{
    public string CombatName;
    public BattleMusic battleMusic;
    public List<CombatAIController> EnemiesInThisCombat;
    public List<CombatAIController> OtherEntitiesInThisCombat;
    public List<CombatRoundEventData> RoundEvents;

    public void Call()
    {
        float music = 1;
        switch ((int)battleMusic)
        {
            case 2:
                //bossMusic
                break;
        }
        CombatEvents.current.StartCombat(CombatName, EnemiesInThisCombat, OtherEntitiesInThisCombat, RoundEvents, music);
    }
}

public enum BattleMusic
{
    defaultBattleTheme = 1,
    bossTheme = 2
}