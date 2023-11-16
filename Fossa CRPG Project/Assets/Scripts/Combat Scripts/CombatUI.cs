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
    [SerializeField] private RectTransform ActionUI;
    [SerializeField] private Button MoveButton;
    [SerializeField] private RectTransform ExamineUI;
    [SerializeField] private RectTransform NeutralUI;
    [SerializeField] private GameObject cancelDisp;

    [SerializeField] private GameObject abilityUI;
    [SerializeField] private List<AbilityButton> abilityButton;

    private bool UIOpen;
    private bool actionActive;
    private bool hasMoved;
    private OverlayTile selectedTile;

    private float currentDelay = 0;
    private static float delay = 0.2f;

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
        if(currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
            return;
        }
        UpdateUI();
        GetActionInput();
    }

    private void GetActionInput()
    {
        if(!UIOpen)
        {
            if (Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0))
            {
                OpenUI();
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (abilityUI.activeInHierarchy && actionActive != true)
                {
                    CloseAbilityUI();
                }
                else if(actionActive)
                {
                    CloseAbilityUI();
                    displayUI(selectedTile);
                    ChangeCursorMode(5);
                    actionActive = false;
                }
                else
                {
                    ChangeCursorMode(1);
                    UIOpen = false;
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
            NeutralUI.gameObject.SetActive(true);
            NeutralUI.GetComponentInChildren<Button>().Select();
        }
        else if(selectedTile.activeCharacter == Char)
        {
            //Display Action UI
            ActionUI.gameObject.SetActive(true);
            if (hasMoved)
            {
                MoveButton.interactable = false;
                ActionUI.GetComponentsInChildren<Button>()[1].Select();
            }
            else { ActionUI.GetComponentInChildren<Button>().Select(); }
        }
        else
        {
            //Display Examine UI
            ExamineUI.gameObject.SetActive(true);
            ExamineUI.GetComponentInChildren<Button>().Select();
        }
    }
    public void OpenUI()
    {
        UIOpen = true;
        ChangeCursorMode(5);

    }
    public void CloseUI()
    {
        ActionUI.gameObject.SetActive(false);
        ExamineUI.gameObject.SetActive(false);
        NeutralUI.gameObject.SetActive(false);
    }
    public void OpenAbilityUI()
    {
        abilityUI.SetActive(true);
        ActionUI.gameObject.SetActive(false);

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
        ActionUI.gameObject.SetActive(true);
    }

    public void CallAbility(int ID)
    {
        foreach (var item in abilityButton) { item.gameObject.SetActive(false); }
        CombatEvents.current.SetCursorMode(4, Char.activeAbilities[ID]);
        actionActive = true;
    }
    private void ActionComplete()
    {
        hasMoved = true;
        actionActive = false;
        displayUI(Char.activeTile);
        ChangeCursorMode(5);
    }

    private void SetupCombatUI(Entity entity)
    {
        Char = entity;
        hasMoved = false;
        MoveButton.interactable = true;

        currentDelay = 0;
        UIOpen = false;
        actionActive = false;

        cancelDisp.SetActive(false);
        ActionUI.gameObject.SetActive(false);
        ExamineUI.gameObject.SetActive(false);
        NeutralUI.gameObject.SetActive(false);
        abilityUI.gameObject.SetActive(false);
        ChangeCursorMode(1);
    }
    public void PositionUI(RectTransform rectTransform)
    {
        var mousePos = Input.mousePosition;
        rectTransform.gameObject.SetActive(true);
        rectTransform.transform.position = new Vector2(mousePos.x + rectTransform.sizeDelta.x, mousePos.y);
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
        if (mode == 2 || mode == 3 || mode == 4)
        {
            actionActive = true;
            cancelDisp.SetActive(true);
            CloseUI();
        }
        else if(mode == 1)
        {
            currentDelay = delay;
            UIOpen = false;
            cancelDisp.SetActive(false);
        }
        else { cancelDisp.SetActive(false); }
        CombatEvents.current.SetCursorMode(mode, null);
    }
}
