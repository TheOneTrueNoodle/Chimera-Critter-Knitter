using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineFreeLook combatCamera;
    [SerializeField] private float cameraRotationSpeed = 1f;

    [SerializeField] private LayerMask combatLayerMask;
    private LayerMask defaultLayerMask;

    private bool inCombat;

    private bool usedCam;
    private bool fadedIn;
    private float camHintTimer = 5f;
    private float currentTimer;

    [SerializeField] private Animator camHintAnim;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;

        Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    private float GetAxisCustom(string axisName)
    {
        if (!inCombat) { return 0; }

        if (usedCam != true)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > camHintTimer)
            {
                fadedIn = true;
                camHintAnim.Play("Fade In");
            }
        }

        if (axisName == "Mouse X")
        {
            if (Input.GetButton("Control Camera"))
            {
                float x = Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("Camera Sensitivity", 3);
                x = PlayerPrefs.GetInt("Invert Camera X", 0) == 1 ? x * -1 : x;

                if (x != 0)
                {
                    usedCam = true;
                    if (fadedIn)
                    {
                        camHintAnim.Play("Fade Out");
                    }
                    else
                    {
                        camHintAnim.gameObject.SetActive(false);
                    }
                }

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
                float y = Input.GetAxis("Mouse Y") * PlayerPrefs.GetFloat("Camera Sensitivity", 3);
                y = PlayerPrefs.GetInt("Invert Camera Y", 0) == 1 ? y * -1 : y;

                if (y != 0) 
                { 
                    usedCam = true; 
                    if (fadedIn)
                    {
                        camHintAnim.Play("Fade Out");
                    }
                    else
                    {
                        camHintAnim.gameObject.SetActive(false);
                    }
                }

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
        if (!usedCam) { currentTimer = 0; }

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
