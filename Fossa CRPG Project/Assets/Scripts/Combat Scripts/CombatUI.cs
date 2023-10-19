using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    public Image portrait;
    public TMP_Text Name;
    public Slider HPBar;
    public TMP_Text HPText;
    public Slider SPBar;
    public TMP_Text SPText;

    private Entity Char;
    private bool started;

    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject abilityUI;
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private List<AbilityButton> abilityButton;

    private void Start()
    {
        CombatEvents.current.onNewTurn += SetupCombatUI;
        CombatEvents.current.onStartCombat += StartCombat;
        CombatEvents.current.onActionComplete += ActionComplete;
    }

    private void Update()
    {
        UpdateUI();

        GetActionInput();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (abilityUI.activeInHierarchy)
            {
                CloseAbilityUI();
                ChangeCursorMode(1);
            }
            else
            {
                ChangeCursorMode(1);
            }
        }
    }

    private void SetupCombatUI(Entity entity)
    {
        Char = entity;

        if (Char.TeamID != 0)
        {
            //ENEMY
            backButton.SetActive(false);
            mainUI.SetActive(false);
            abilityUI.SetActive(false);
            endTurnButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(false);
            mainUI.SetActive(true);
            abilityUI.SetActive(false);
            endTurnButton.SetActive(true);
        }
    }

    private void GetActionInput()
    {

    }

    private void StartCombat()
    {
        started = true;
    }

    public void UpdateUI()
    {
        if (!started) { return; }
        portrait.sprite = Char.CharacterData.portrait;
        Name.text = Char.CharacterData.Name;
        HPBar.maxValue = (int)Char.activeStatsDir["MaxHP"].baseStatValue;
        HPBar.value = Char.activeStatsDir["MaxHP"].statValue;
        HPText.text = HPBar.value.ToString() + " / " + HPBar.maxValue.ToString();
        SPBar.maxValue = (int)Char.activeStatsDir["MaxSP"].baseStatValue;
        SPBar.value = Char.activeStatsDir["MaxSP"].statValue;
        SPText.text = SPBar.value.ToString() + " / " + SPBar.maxValue.ToString();
    }

    public void ChangeCursorMode(int mode)
    {
        if (mode != 1)
        {
            backButton.SetActive(true);
            mainUI.SetActive(false);
        }

        if (mode != 4)
        {
            mainUI.SetActive(true);
            backButton.SetActive(false);
        }

        if (mode == 5)
        {
            mainUI.SetActive(false);
            backButton.SetActive(false);
        }
        CombatEvents.current.SetCursorMode(mode, null);
    }

    public void OpenAbilityUI()
    {
        abilityUI.SetActive(true);
        mainUI.SetActive(false);

        foreach (var button in abilityButton)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < Char.activeAbilities.Count; i++)
        {
            abilityButton[i].gameObject.SetActive(true);
            abilityButton[i].SetupButton(Char.activeAbilities[i]);
        }
    }

    public void CloseAbilityUI()
    {
        foreach (var item in abilityButton) { item.gameObject.SetActive(false); }
        abilityUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void CallAbility(int ID)
    {
        foreach (var item in abilityButton) { item.gameObject.SetActive(false); }
        CombatEvents.current.SetCursorMode(4, Char.activeAbilities[ID]);
    }

    public void endTurn()
    {
        CombatEvents.current.TurnEnd();
    }

    private void ActionComplete()
    {
        mainUI.SetActive(true);
        backButton.SetActive(false);
        CombatEvents.current.SetCursorMode(0, null);
    }
}
