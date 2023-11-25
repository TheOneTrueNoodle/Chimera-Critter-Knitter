using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    [Header("Unit Information Display")]
    public UnitInfoUI currentUnitUI;
    public UnitInfoUI selectedUnitUI;

    private Entity Char;
    private bool started;

    [Header("Functionality")]
    [SerializeField] private GameObject UIParent;
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

    private CursorController cursor;

    private void Start()
    {
        CombatEvents.current.onNewTurn += SetupCombatUI;
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        CombatEvents.current.onActionComplete += ActionComplete;
        CombatEvents.current.onGetSelectedTile += displayUI;

        cursor = FindObjectOfType<CursorController>();
    }

    private void Update()
    {
        if (!started) { return; }
        if(currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
            return;
        }
        UpdateInformationUI();
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
                    displayUI(selectedTile);
                    ChangeCursorMode(5);
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

        if (Char.activeAbilities.Count > 0)
        {
            abilityButton[0].GetComponent<Button>().Select();
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
        actionActive = true;
        cancelDisp.SetActive(true);
        foreach (var item in abilityButton) { item.gameObject.SetActive(false); }
        CombatEvents.current.SetCursorMode(4, Char.activeAbilities[ID]);
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
    public void UpdateInformationUI()
    {
        if (!started) { return; }
        var selectedTile = cursor.currentTile;

        currentUnitUI.UpdateUI(Char);

        if (selectedTile != null)
        {
            if(selectedTile.activeCharacter != null && selectedTile.activeCharacter != Char)
            {
                //Display ui of selected character
                selectedUnitUI.UpdateUI(selectedTile.activeCharacter);
                if (!selectedUnitUI.gameObject.activeInHierarchy)
                {
                    selectedUnitUI.gameObject.SetActive(true);
                }
            }
            else
            {
                //Hide unit UI
                if (selectedUnitUI.gameObject.activeInHierarchy)
                {
                    selectedUnitUI.gameObject.SetActive(false);
                }
            }
        }
    }
    private void StartCombat()
    {
        started = true;
        UIParent.SetActive(true);
    }
    private void EndCombat()
    {
        started = false;
        UIParent.SetActive(false);
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
