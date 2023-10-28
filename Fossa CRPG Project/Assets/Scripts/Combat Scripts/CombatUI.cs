using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    [Header("Unit Information Display")]
    public Image portrait;
    public TMP_Text Name;
    public Slider HPBar;
    public TMP_Text HPText;
    public Slider SPBar;
    public TMP_Text SPText;

    private Entity Char;
    private bool started;

    [Header("Functionality")]
    [SerializeField] private GameObject ActionUI;
    [SerializeField] private GameObject ExamineUI;
    [SerializeField] private GameObject NeutralUI;

    [SerializeField] private GameObject abilityUI;
    [SerializeField] private List<AbilityButton> abilityButton;

    private bool UIOpen;
    private OverlayTile selectedTile;

    private void Start()
    {
        CombatEvents.current.onNewTurn += SetupCombatUI;
        CombatEvents.current.onStartCombat += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        CombatEvents.current.onActionComplete += ActionComplete;
        CombatEvents.current.onGetSelectedTile += displayUI;
    }

    private void Update()
    {
        if (!started) { return; }
        UpdateUI();
        GetActionInput();
    }

    private void GetActionInput()
    {
        if(!UIOpen)
        {
            if (Input.GetButtonDown("Interact"))
            {
                OpenUI();
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (abilityUI.activeInHierarchy)
                {
                    CloseAbilityUI();
                }
                else
                {
                    ChangeCursorMode(1);
                    CloseUI();
                }
            }
        }
    }

    public void displayUI(OverlayTile tile)
    {
        selectedTile = tile;
        if (selectedTile.isBlocked != true)
        {
            //Open empty tile UI
            NeutralUI.SetActive(true);
            NeutralUI.GetComponentInChildren<Button>().Select();
        }
        else if(selectedTile.activeCharacter == Char)
        {
            //Display Action UI
            ActionUI.SetActive(true);
            ActionUI.GetComponentInChildren<Button>().Select();
        }
        else
        {
            //Display Examine UI
            ExamineUI.SetActive(true);
            ExamineUI.GetComponentInChildren<Button>().Select();
        }
    }
    public void OpenUI()
    {
        Debug.Log("Opening UI");
        UIOpen = true;
        ChangeCursorMode(5);
    }
    public void CloseUI()
    {
        UIOpen = false;
        ActionUI.SetActive(false);
        ExamineUI.SetActive(false);
        NeutralUI.SetActive(false);
    }
    public void OpenAbilityUI()
    {
        abilityUI.SetActive(true);
        ActionUI.SetActive(false);

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
        ActionUI.SetActive(true);
    }

    public void CallAbility(int ID)
    {
        foreach (var item in abilityButton) { item.gameObject.SetActive(false); }
        CombatEvents.current.SetCursorMode(4, Char.activeAbilities[ID]);
    }
    private void ActionComplete()
    {
        CombatEvents.current.SetCursorMode(1, null);
    }
    private void SetupCombatUI(Entity entity)
    {
        Char = entity;

        UIOpen = false;

        ActionUI.SetActive(false);
        ExamineUI.SetActive(false);
        NeutralUI.SetActive(false);
        abilityUI.SetActive(false);
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
    private void StartCombat()
    {
        started = true;
    }
    private void EndCombat()
    {
        started = false;
    }
    public void ChangeCursorMode(int mode)
    {
        CombatEvents.current.SetCursorMode(mode, null);
    }
}
