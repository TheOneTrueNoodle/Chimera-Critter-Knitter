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

    private bool menuOpen;
    private bool pauseMenuOpen;

    [SerializeField] private CharacterMenuManager characterMenuManager;
    [SerializeField] private MutationMenu mutationMenuManager;

    [Header("Popups")]
    [SerializeField] private GameObject popupParent;
    public Popup popupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onStartCombatSetup += StartCombat;
            CombatEvents.current.onEndCombat += EndCombat;
            CombatEvents.current.onPauseGame += CombatPauseMenuInput;
        }
        if (MenuEvent.current != null)
        {
            MenuEvent.current.onSpawnPopup += SpawnPopup;
        }
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat) { return; }

        if (Input.GetButtonDown("Character Menu") && !pauseMenuOpen)
        {
            //Open or close the character menu
            if (!menuOpen)
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
            if(menuOpen)
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
        SceneManager.LoadScene("Alpha");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCharacterMenu()
    {
        characterMenuObj.SetActive(true);

        characterMenuManager.UpdateDisplay();
        mutationMenuManager.UpdateDisplay();

        Time.timeScale = 0;
        menuOpen = true;
    }

    public void CloseCharacterMenu()
    {
        characterMenuObj.SetActive(false);
        Time.timeScale = 1;
        menuOpen = false;
    }

    public void SpawnPopup(string text)
    {
        var popup = Instantiate(popupPrefab, popupParent.transform);
        popup.Setup(text);
    }

    private void StartCombat(string combatName)
    {
        inCombat = true; 
        if (menuOpen)
        {
            CloseCharacterMenu();
        }
    }
    private void EndCombat(string combatName)
    {
        inCombat = false;
    }
}
