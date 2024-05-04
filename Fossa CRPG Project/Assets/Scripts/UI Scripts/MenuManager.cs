using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject OpeningScreen;
    public Animator SettingsAnim;
    public GameObject SettingsScreen;
    public GameObject SplashScreen;

    public GameObject buttons;
    public GameObject ControlsScreen;

    public Animator sceneTransition;

    void Start()
    {
        StartCoroutine(OpeningGame());

        DontDestroyOnLoad(this);
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
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        sceneTransition.gameObject.SetActive(true);
        sceneTransition.Play("FadeOut");

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(sceneTransition.GetCurrentAnimatorStateInfo(0).length);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Alpha", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(currentScene);

        sceneTransition.Play("FadeIn");
        yield return new WaitForSecondsRealtime(sceneTransition.GetCurrentAnimatorStateInfo(0).length);

        Time.timeScale = 1;

        Destroy(this);
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
