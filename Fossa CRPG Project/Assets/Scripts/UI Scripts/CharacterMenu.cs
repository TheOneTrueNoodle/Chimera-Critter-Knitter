using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private bool inCombat;

    private bool menuOpen;

    [SerializeField] public GameObject MenuObject;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }
    private void Update()
    {
        if (inCombat)
        {
            return;
        }
        
    }

    private void StartCombat(string combatName)
    {
        inCombat = true;

        //If menu is open close the menu
        
    }
    private void EndCombat(string combatName)
    {
        inCombat = false;
    }
}
