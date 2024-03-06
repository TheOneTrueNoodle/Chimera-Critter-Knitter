using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject OpeningScreen;
    public GameObject SettingsScreen;
    public GameObject FirstCutscene;
    public GameObject SplashScreen;

    void Start()
    {
        StartCoroutine(OpeningGame());
    }

    public void StartGame()
    {
        StartCoroutine(StartingCutscene());
        
    }

    IEnumerator StartingCutscene()
    {
        FirstCutscene.SetActive(true);
        OpeningScreen.SetActive(false);
        Debug.Log("StartingCutscene");
        yield return new WaitForSeconds(3f);
        Debug.Log("EndingCutscene");
        SceneManager.LoadScene("Whitebox");
    }

    IEnumerator OpeningGame()
    {
        
        Debug.Log("StartingSplashScreen");
        yield return new WaitForSeconds(3f);
        Debug.Log("SplashScreenEnding");
       OpeningScreen.SetActive(true);
        SplashScreen.SetActive(false);

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