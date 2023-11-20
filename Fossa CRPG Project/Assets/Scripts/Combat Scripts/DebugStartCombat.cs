using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStartCombat : MonoBehaviour
{
    public void Call()
    {
        CombatEvents.current.StartCombat(null, null);
    }
}
