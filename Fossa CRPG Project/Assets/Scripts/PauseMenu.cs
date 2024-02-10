using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject SettingsMenuImage;

    private bool inCombat;
   
    // Start is called before the first frame update
    void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        CombatEvents.current.onPauseGame += CombatPauseMenuInput;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat) { return; }

        if (Input.GetButtonDown("Menu"))
        {
            if (SettingsMenuImage.activeInHierarchy)
            {
                SettingsBack();
            }
            else if(pauseMenu.activeInHierarchy)
            {
                unPause();
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void CombatPauseMenuInput()
    {
        if (pauseMenu.activeInHierarchy)
        {
            unPause();
        }
        else if (SettingsMenuImage.activeInHierarchy)
        {
            SettingsBack();
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void unPause()
    {
        pauseMenu.SetActive(false);
        SettingsMenuImage.SetActive(false);
        Time.timeScale = 1;

        if (inCombat) { CombatEvents.current.UnpauseGame(); }
    }

    public void SettingsMenu()
    {
        pauseMenu.SetActive(false);
        SettingsMenuImage.SetActive(true);
    }

    public void SettingsBack()
    {
        pauseMenu.SetActive(true);
        SettingsMenuImage.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Demo");
    }

    private void StartCombat()
    {
        inCombat = true;
    }
    private void EndCombat()
    {
        inCombat = false;
    }
}
