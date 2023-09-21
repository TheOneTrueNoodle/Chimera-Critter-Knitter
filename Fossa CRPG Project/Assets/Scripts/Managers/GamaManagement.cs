using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManagement : MonoBehaviour
{
    public GameData gm;
    public string fileNumber;
    private GameObject debugWindow;
    private GameObject debugText;
    private TMP_Text textMesh;

    [Header("OPTIONS")]
    public bool saveOnApplicationQuit;
    public bool showDebuggingTools;
    public bool showDebuggingText;


    void Start()
    {
        UpdateSceneFromManager();

        debugWindow = GameObject.Find("Game Data");
        //textMesh = GameObject.Find("Display Data").GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (showDebuggingTools)
        {
            ShowDebugging();
        }
        else
        {
            //debugWindow.SetActive(false);
        }
    }

    public void ResetGame()
    {
        gm.setAsDefaultStatus(); //set values back to defaults
        UpdateSceneFromManager(); //re-update the scene using those values

        if (showDebuggingText) { Debug.Log("Resetting"); }
    }


    void OnApplicationQuit()
    {
        if (saveOnApplicationQuit)
        {
            SaveFromSceneToManager();
            gm.SaveGameStatus();

            if (showDebuggingText) { Debug.Log("Saved On Quit"); }
        }
        else
        {
            if (showDebuggingText) { Debug.Log("Not Saved On Quit"); }
        }
    }

    public void SaveFromSceneToManager() //what are we saving?
    {
        gm.currentGameStatus.playerPosition = GameObject.Find("PLAYER").transform.position;

        gm.SaveGameStatus();
        if (showDebuggingText) { Debug.Log("Saving..."); }
    }


    public void UpdateSceneFromManager() //load last thing saved
    {
        //GameObject.Find("PLAYER").transform.position = gm.currentGameStatus.playerPosition;
        gm.LoadGameStatus();
        if (showDebuggingText) { Debug.Log("Updating Scene Variables From Manager"); }
    }

    void ShowDebugging()
    {
        //debugWindow.SetActive(true);
        //textMesh.text = "Saved Data: " + gm.currentGameStatus.playerPosition.ToString();

    }

    public void SetFileNumber(string buttonpressed)
    {
        gm.fileNR = buttonpressed;
        //gm.SaveGameStatus();
    }
}
