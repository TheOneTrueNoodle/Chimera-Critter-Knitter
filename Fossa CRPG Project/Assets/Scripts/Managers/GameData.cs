using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct GameStatus
{
    //all data to be stored
    public bool clickedButton;
    public Vector3 playerPosition;
    public string nextFight;

    public int health;
    public int stress;
    public int maxHealth;
    public int maxStress;
}

public class GameData : MonoBehaviour
{

    public GameStatus currentGameStatus;
    string filePath;
    public string fileNR = "5";
    string FILE_NAME;
    const string RESET_FILE_NAME = "ResetSaveStatus.json";

    public static readonly GameData instance = new GameData();

    private GameData()
    {
    }

    public static GameData Instance
    {
        get
        {
            return instance;
        }
    }


    public void Awake()
    {
        FILE_NAME = "SaveStatus" + fileNR + ".json";
        //retrieving saving location
        filePath = Application.persistentDataPath;
        currentGameStatus = new GameStatus();
        //Debug.Log(filePath);

    }

    public void SaveGameStatus()    //override save file
    {
        FILE_NAME = "SaveStatus" + fileNR + ".json";

        //serialise the GameStatus struct into a Json string
        string gameStatusJson = JsonUtility.ToJson(currentGameStatus);
        //write a text file containing the string value as simple text
        File.WriteAllText(filePath + "/" + FILE_NAME, gameStatusJson);
        Debug.Log("File created and saved");
    }

    public void LoadGameStatus() //if json exists, read it and load
    {
        FILE_NAME = "SaveStatus" + fileNR + ".json";

        //check the file exists
        if (File.Exists(filePath + "/" + FILE_NAME))
        {
            //load the file content as string
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            //deserialise the loaded string into a GameStatus struct
            currentGameStatus = JsonUtility.FromJson<GameStatus>(loadedJson);

            Debug.Log("File" + FILE_NAME + "loaded successfully");
        }
        else
        {
            //initilise a new game status
            setAsDefaultStatus();

            Debug.Log("File not found: overwritten with default");
        }
    }

    public void setAsDefaultStatus() //sets all variables back to their default position
    {
        //initilise a new game status
        currentGameStatus.clickedButton = false;
        currentGameStatus.playerPosition = new Vector3(0, -1, 0);

        //player stats
        currentGameStatus.health = 10;
        currentGameStatus.stress = 30;
        currentGameStatus.maxHealth = 10;
        currentGameStatus.maxStress = 30;

        SaveGameStatus();
        Debug.Log("Set to default game status");
    }
}
