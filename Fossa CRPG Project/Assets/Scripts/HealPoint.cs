using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    public void Call(Entity oscar)
    {
        CombatEvents.current.HealAttempt(oscar, (int)oscar.activeStatsDir["MaxHP"].baseStatValue);
        oscar.activeStatsDir["MaxSP"].statValue = oscar.activeStatsDir["MaxSP"].baseStatValue;
        MenuEvent.current.SpawnPopup("You ate some food and fully healed!");

        //Play some sort of green effect to show healing
    }
}
