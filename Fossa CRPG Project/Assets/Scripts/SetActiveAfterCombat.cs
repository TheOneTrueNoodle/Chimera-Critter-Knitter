using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterCombat : MonoBehaviour
{
    public List<GameObject> objs;
    private void Start()
    {
        CombatEvents.current.onEndCombat += SetActive;
    }

    private void SetActive()
    {
        foreach (var obj in objs)
        {
            obj.SetActive(true);
        }
    }
}
