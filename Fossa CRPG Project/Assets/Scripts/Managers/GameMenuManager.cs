using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    private bool inCombat;

    [SerializeField] public GameObject characterMenuObj;

    public GameObject pauseMenuObj;
    public GameObject SettingsMenuObj;

    private bool characterMenuOpen;
    private bool pauseMenuOpen;


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

        if (Input.GetButtonDown("Character Menu") && !pauseMenuOpen)
        {
            //Open or close the character menu
            if (!characterMenuOpen)
            {
                OpenCharacterMenu();
            }
            else
            {
                CloseCharacterMenu();
            }
        }

        if (Input.GetButtonDown("Pause"))
        {
            if(characterMenuOpen)
            {
                CloseCharacterMenu();
            }
            else if (SettingsMenuObj.activeInHierarchy)
            {
                SettingsBack();
            }
            else if (pauseMenuObj.activeInHierarchy)
            {
                unPause();
            }
            else
            {
                pauseMenuOpen = true;
                pauseMenuObj.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void CombatPauseMenuInput()
    {
        
        if (SettingsMenuObj.activeInHierarchy)
        {
            SettingsBack();
        }
        else if (pauseMenuObj.activeInHierarchy)
        {
            unPause();
        }
        else
        {
            pauseMenuObj.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void unPause()
    {
        pauseMenuObj.SetActive(false);
        SettingsMenuObj.SetActive(false);
        Time.timeScale = 1;

        pauseMenuOpen = false;

        if (inCombat) { CombatEvents.current.UnpauseGame(); }
    }

    public void SettingsMenu()
    {
        pauseMenuObj.SetActive(false);
        SettingsMenuObj.SetActive(true);
    }

    public void SettingsBack()
    {
        pauseMenuObj.SetActive(true);
        SettingsMenuObj.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Demo");
    }

    public void OpenCharacterMenu()
    {
        characterMenuObj.SetActive(true);
        Time.timeScale = 0;
        characterMenuOpen = true;
    }

    public void CloseCharacterMenu()
    {
        characterMenuObj.SetActive(false);
        Time.timeScale = 1;
        characterMenuOpen = false;
    }

    private void StartCombat()
    {
        inCombat = true; 
        if (characterMenuOpen)
        {
            CloseCharacterMenu();
        }
    }
    private void EndCombat()
    {
        inCombat = false;
    }
}
