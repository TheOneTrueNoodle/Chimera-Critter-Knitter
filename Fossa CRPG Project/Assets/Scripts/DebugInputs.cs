using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DebugInputs : MonoBehaviour
{
    private int counter = 0;
    public GameObject WinScreen;
    public CinemachineBrain cinemachineBrain;
    public ICinemachineCamera postprocessingcamera;
    public ICinemachineCamera nopostprocessingcamera;
 
    void Update()
    {
        if (Input.GetButtonDown("Debug Reset"))
        {
            ReloadScene();
        }
    }

    public void Call()
    {
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

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Demo");
    }
}
