using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    private bool inCombat;

    [SerializeField] public Animator characterMenuObj;

    public GameObject pauseMenuObj;
    public SettingsManager SettingsMenuObj;

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
            else if (SettingsMenuObj.gameObject.activeInHierarchy)
            {
                //Handle in Settings Menu object;
                SettingsMenuObj.TryCloseSettings();
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
        
        if (SettingsMenuObj.gameObject.activeInHierarchy)
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
        SettingsMenuObj.gameObject.SetActive(false);
        Time.timeScale = 1;

        pauseMenuOpen = false;

        if (inCombat) { CombatEvents.current.UnpauseGame(); }
    }

    public void SettingsMenu()
    {
        pauseMenuObj.SetActive(false);
        SettingsMenuObj.gameObject.SetActive(true);
    }

    public void SettingsBack()
    {
        pauseMenuObj.SetActive(true);
        SettingsMenuObj.gameObject.SetActive(false);
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
        MenuEvent.current.OpenMenu();
        characterMenuObj.Play("Open Menu");

        characterMenuManager.UpdateDisplay();
        mutationMenuManager.UpdateDisplay();

        menuOpen = true;
    }

    public void CloseCharacterMenu()
    {
        MenuEvent.current.CloseMenu();
        characterMenuObj.Play("Close Menu");
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
