using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DebugInputs : MonoBehaviour
{
    public int counter = 0;
    public GameObject WinScreen;
    public CinemachineBrain cinemachineBrain;
    public ICinemachineCamera postprocessingcamera;
    public ICinemachineCamera nopostprocessingcamera;

    public void Call()
    {
        Debug.Log("Called");
        counter++;
        if (counter >= 2)
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }
}
