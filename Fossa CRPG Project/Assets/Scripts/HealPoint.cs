using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    public void Call(Entity oscar)
    {
        CombatEvents.current.HealAttempt(oscar, (int)oscar.activeStatsDir["MaxHP"].baseStatValue);
        MenuEvent.current.SpawnPopup("You ate some food and healed!");

        //Play some sort of green effect to show healing
    }
}
