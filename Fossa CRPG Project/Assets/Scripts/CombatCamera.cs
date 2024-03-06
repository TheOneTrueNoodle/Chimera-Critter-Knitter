using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera combatCamera;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    private void StartCombat(string combatName)
    {
        combatCamera.Priority = 20;
    }
    private void EndCombat(string combatName)
    {
        combatCamera.Priority = 9;
    }
}
