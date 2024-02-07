using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject OpeningScreen;
    public GameObject SettingsScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Demo");
    }

    public void Settings()
    {
        OpeningScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }

    public void SettingsBack()
    {
        OpeningScreen.SetActive(true);
        SettingsScreen.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("QuittingGame");
    }
}
