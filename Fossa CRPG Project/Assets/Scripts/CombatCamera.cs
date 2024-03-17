using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineFreeLook combatCamera;
    [SerializeField] private float cameraRotationSpeed = 2f;

    private bool inCombat;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;

        Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    private float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetButton("Control Camera"))
            {
                return Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            return 0;
        }
        return Input.GetAxis(axisName);
    }
    private void StartCombat(string combatName)
    {
        combatCamera.Priority = 20;
        inCombat = true;
    }
    private void EndCombat(string combatName)
    {
        combatCamera.Priority = 9;
        inCombat = false;
    }
}
