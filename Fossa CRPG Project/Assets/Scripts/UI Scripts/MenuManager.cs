using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject OpeningScreen;
    public Animator SettingsAnim;
    public GameObject SettingsScreen;
    public GameObject FirstCutscene;
    public GameObject SplashScreen;

    public GameObject buttons;
    public GameObject ControlsScreen;

    void Start()
    {
        StartCoroutine(OpeningGame());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (ControlsScreen.activeInHierarchy)
            {
                CloseControlsMenu();
            }
        }
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
        SceneManager.LoadScene("Alpha");
    }

    IEnumerator OpeningGame()
    {
        SplashScreen.SetActive(true);
        Debug.Log("StartingSplashScreen");
        yield return new WaitForSeconds(4.8f);
        Debug.Log("SplashScreenEnding");
       // OpeningScreen.SetActive(true);
        SplashScreen.SetActive(false);

    }

    public void OpenControlsMenu()
    {
        ControlsScreen.SetActive(true);
        buttons.SetActive(false);
    }
    public void CloseControlsMenu()
    {
        ControlsScreen.SetActive(false);
        buttons.SetActive(true);
    }


    public void Settings()
    {
        SettingsAnim.Play("Open");
    }

    public void SettingsBack()
    {
        SettingsAnim.Play("Close");
    }

    public void Quit()
    {
        Debug.Log("QuittingGame");
        Application.Quit();
    }
}
