using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DebugInputs : MonoBehaviour
{
    public int counter = 0;
    public GameObject WinScreen;

    public void Call()
    {
        Debug.Log("Called");
        counter++;
        if (counter >= 3)
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }
}
