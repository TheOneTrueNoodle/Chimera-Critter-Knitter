using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimsOnCombatEnd : MonoBehaviour
{
    [SerializeField] private string combatName;
    [SerializeField] private Animator anim;

    private void Start()
    {
        CombatEvents.current.onEndCombat += EndCombat;
    }

    private void EndCombat(string name)
    {
        if(name == combatName)
        {
            anim.enabled = false;
        }    
    }
}
