using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterCombat : MonoBehaviour
{
    public string combat;
    public List<GameObject> objs;
    private void Start()
    {
        CombatEvents.current.onEndCombat += SetActive;
    }

    private void SetActive(string combatName)
    {
        if(combatName != combat) { return; }
        foreach (var obj in objs)
        {
            obj.SetActive(true);
        }
        objs.Clear();
    }
}
