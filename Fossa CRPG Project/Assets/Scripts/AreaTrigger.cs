using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private bool inCombat;
    public bool oneTimeUse; 
    
    private bool used = false;
    public Interaction Interaction;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (inCombat) { return; }
        if (!used && other.gameObject.CompareTag("Player"))
        {
            if (oneTimeUse == true)
            {
                Interaction.Invoke();
                used = true;
            }
            else
            {
                Interaction.Invoke();
            }
        }
    }
    private void StartCombat()
    {
        inCombat = true;
    }
    private void EndCombat()
    {
        inCombat = false;
    }
}
