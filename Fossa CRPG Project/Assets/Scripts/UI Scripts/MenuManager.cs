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
    public GameObject CreditsScreen;

    //Variables for loading main scene
    private bool loadingGame; 

    public Animator sceneTransition;
    public Animator controlsTransition;

    private bool closeControlsTransition;

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

            if (loadingGame)
            {
                CloseControlsWhileLoading();
            }
        }

    }

    public void StartGame()
    {
        Destroy(buttons);
        loadingGame = true;
        StartCoroutine(LoadGame());
    }

    public void CloseControlsWhileLoading()
    {
        closeControlsTransition = true;
    }

    IEnumerator LoadGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        controlsTransition.gameObject.SetActive(true);
        controlsTransition.Play("ControlsFadeIn");

        float passedTime = 0;

        while(!closeControlsTransition && passedTime < 10)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }

        controlsTransition.Play("ControlsFadeOut");
        yield return new WaitForSecondsRealtime(controlsTransition.GetCurrentAnimatorStateInfo(0).length);

        sceneTransition.gameObject.SetActive(true);
        sceneTransition.Play("FadeOut");
        yield return new WaitForSecondsRealtime(sceneTransition.GetCurrentAnimatorStateInfo(0).length);

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
    
    public void OpenCreditsMenu()
    {
        CreditsScreen.SetActive(true);
        buttons.SetActive(false);
    }
    public void CloseCreditsMenu()
    {
        CreditsScreen.SetActive(false);
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
