using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera combatCamera;
    [SerializeField] private float cameraRotationSpeed = 2f;

    private Vector3 oldMousePosition;

    private bool inCombat;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    void Update()
    {
        //if (!inCombat) { return; }
        if (Input.GetButtonDown("Control Camera"))
        {
            Debug.Log("Pressed right button");
            oldMousePosition = Input.mousePosition;
            return;
        }

        if (Input.GetButton("Control Camera"))
        {
            Debug.Log("Held right button");
            Vector3 currentMousePosition = Input.mousePosition;

            if (currentMousePosition.x < oldMousePosition.x)
            {
                float x = combatCamera.transform.eulerAngles.x;
                float y = combatCamera.transform.eulerAngles.y;
                combatCamera.transform.eulerAngles = new Vector3(x, y + cameraRotationSpeed);
            }

            if (currentMousePosition.x > oldMousePosition.x)
            {
                float x = combatCamera.transform.eulerAngles.x;
                float y = combatCamera.transform.eulerAngles.y;
                combatCamera.transform.eulerAngles = new Vector3(x, y - cameraRotationSpeed);
            }
        }
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
