using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;

    [SerializeField] public Animator characterMenuObj;

    public GameObject journalButton;

    public GameObject pauseMenuObj;
    public SettingsManager SettingsMenuObj;
    public GameObject controlsMenuObj;
    private bool controlsOpen;

    private bool menuOpen;
    private bool pauseMenuOpen;

    [SerializeField] private CharacterMenuManager characterMenuManager;
    [SerializeField] private MutationMenu mutationMenuManager;

    [Header("Popups")]
    [SerializeField] private GameObject popupParent;
    public Popup popupPrefab;

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private UnityEngine.UI.Image interactSpriteImage;
    [SerializeField] private UnityEngine.UI.Image interactSpriteShadow;

    [Header("Audio")]
    [field: SerializeField] private FMODUnity.EventReference openCharacterMenuSFX;
    [field: SerializeField] private FMODUnity.EventReference closeCharacterMenuSFX;

    // Start is called before the first frame update
    void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onStartCombatSetup += StartCombat;
            CombatEvents.current.onPauseGame += CombatPauseMenuInput;
        }
        if (DialogueEvents.current != null)
        {
            DialogueEvents.current.onStartDialogue += StartDialogue;
        }
        if (MenuEvent.current != null)
        {
            MenuEvent.current.onSpawnPopup += SpawnPopup;
            MenuEvent.current.onShowInteractUI += ShowInteractUI;
            MenuEvent.current.onHideInteractUI += HideInteractUI;
        }
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat) { return; }
        if (!inDialogue)
        {
            if (Input.GetButtonDown("Character Menu") && !pauseMenuOpen)
            {
                CharacterMenuInput();
            }
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (controlsOpen)
            {
                CloseControlsMenu();
            }
            else if(menuOpen)
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

    public void CharacterMenuInput()
    {
        if (!pauseMenuOpen)
        {
            //Open or close the character menu
            if (!menuOpen)
            {
                AudioManager.instance.PlayOneShot(openCharacterMenuSFX, transform.position);
                mutationMenuManager.exclamation.SetActive(false);
                OpenCharacterMenu();
            }
            else
            {
                AudioManager.instance.PlayOneShot(closeCharacterMenuSFX, transform.position);
                CloseCharacterMenu();
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
        SettingsMenuObj.GetCurrentSettings();
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

    public void OpenControlsMenu()
    {
        pauseMenuObj.SetActive(false);
        controlsMenuObj.SetActive(true);
        controlsOpen = true;
    }
    public void CloseControlsMenu()
    {
        pauseMenuObj.SetActive(true);
        controlsMenuObj.SetActive(false);
        controlsOpen = false;
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

    public void ShowInteractUI(Interactable interactable)
    {
        if (inDialogue || inCombat) { return; }
        if (interactable != null)
        {
            interactSpriteImage.color = new Color(1, 1, 1, 1);
            interactSpriteShadow.color = Color.black;

            if (interactable.TryGetComponent(out MutationTrigger mutTrig))
            {
                if (!mutationMenuManager.equippedMutations.Contains(mutTrig.requiredMutation))
                {
                    interactSpriteImage.color = new Color(0, 0, 0, 1);
                }
            }

            interactSpriteImage.sprite = interactable.interactSprite;
            interactSpriteImage.preserveAspect = true;
            interactSpriteShadow.sprite = interactable.interactSprite;
            interactSpriteShadow.preserveAspect = true;
        }
        else
        {
            interactSpriteImage.sprite = null;
            interactSpriteShadow.sprite = null;
            interactSpriteImage.color = new Color(1, 1, 1, 0);
            interactSpriteShadow.color = new Color(1, 1, 1, 0);
        }

        interactionUI.SetActive(true);
    }

    public void HideInteractUI()
    {
        interactSpriteImage.sprite = null;
        interactSpriteShadow.sprite = null;
        interactSpriteImage.color = new Color(1, 1, 1, 0);
        interactSpriteShadow.color = new Color(1, 1, 1, 0);
        interactionUI.SetActive(false);
    }

    private void StartCombat(string combatName)
    {
        inCombat = true; 
        if (menuOpen)
        {
            CloseCharacterMenu();
        }

        journalButton.SetActive(false);

        CombatEvents.current.onEndCombat += EndCombat;
    }
    private void EndCombat(string combatName)
    {
        inCombat = false;

        journalButton.SetActive(true);

        CombatEvents.current.onEndCombat -= EndCombat;
    }

    private void StartDialogue()
    {
        inDialogue = true;
        HideInteractUI();

        journalButton.SetActive(false);

        DialogueEvents.current.onEndDialogue += EndDialogue;
    }
    private void EndDialogue()
    {
        inDialogue = false;

        journalButton.SetActive(true);

        DialogueEvents.current.onEndDialogue -= EndDialogue;
    }
}
