using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineFreeLook combatCamera;
    [SerializeField] private float cameraRotationSpeed = 2f;

    [SerializeField] private LayerMask combatLayerMask;
    private LayerMask defaultLayerMask;

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
                float x = Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("Camera Sensitivity");
                x = PlayerPrefs.GetInt("Invert Camera X") == 1 ? x * -1 : x;

                return x;
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetButton("Control Camera"))
            {
                float y = Input.GetAxis("Mouse Y") * PlayerPrefs.GetFloat("Camera Sensitivity");
                y = PlayerPrefs.GetInt("Invert Camera Y") == 1 ? y * -1 : y;

                return y;
            }
            else
            {
                return 0;
            }
        }
        return Input.GetAxis(axisName);
    }
    private void StartCombat(string combatName)
    {
        combatCamera.Priority = 20;
        inCombat = true;

        defaultLayerMask = Camera.main.cullingMask;
        Camera.main.cullingMask = combatLayerMask;
    }
    private void EndCombat(string combatName)
    {
        combatCamera.Priority = 9;
        inCombat = false;

        Camera.main.cullingMask = defaultLayerMask;
    }
}
