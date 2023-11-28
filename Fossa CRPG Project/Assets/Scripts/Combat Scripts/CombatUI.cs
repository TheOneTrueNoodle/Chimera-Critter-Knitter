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
    [SerializeField] private GameObject cancelDisp;

    [SerializeField] private GameObject abilityUI;
    [SerializeField] private List<AbilityButton> abilityButton;

    [Header("End Turn UI")]
    [SerializeField] private Button EndTurnButton;
    [SerializeField] private Image RingFill;

    private float EndTurnFill;

    private bool UIOpen;
    private bool actionActive;
    private bool hasMoved;
    private OverlayTile selectedTile;

    private CursorController cursor;

    private void Start()
    {
        CombatEvents.current.onNewTurn += SetupCombatUI;
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        CombatEvents.current.onActionComplete += ActionComplete;
        //CombatEvents.current.onGetSelectedTile += displayUI;

        cursor = FindObjectOfType<CursorController>();
    }

    private void Update()
    {
        if (!started) { return; }
        UpdateInformationUI();
        GetActionInput();
    }

    private void GetActionInput()
    {
        if(Char.TeamID != 0) { return; }
        if(!UIOpen)
        {
            if (Input.GetButtonDown("Cancel"))
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
                    OpenUI();
                    ChangeCursorMode(5);
                }
                else if(actionActive)
                {
                    CloseAbilityUI();
                    //displayUI(selectedTile);
                    OpenUI();
                    actionActive = false;
                }
            }
        }

        //End Turn Input
        if (Input.GetButton("End Turn"))
        {
            EndTurnFill += Time.deltaTime * 2f;
            RingFill.fillAmount = EndTurnFill;
            if (EndTurnFill >= 1)
            {
                EndTurnFill = 0;
                RingFill.fillAmount = EndTurnFill;
                EndTurnFill += Time.deltaTime * 2f;
            }
        }
        else if (EndTurnFill > 0)
        {
            EndTurnFill -= Time.deltaTime * 2f;
            if(EndTurnFill < 0) { EndTurnFill = 0; }
            RingFill.fillAmount = EndTurnFill;
        }
    }
    public void OpenUI()
    {
        UIOpen = true;
        //Display Action UI
        ActionUI.gameObject.SetActive(true);
        PositionUI(ActionUI, Char.gameObject.transform.position);
        if (ActionUI.TryGetComponent(out Animator anim))
        {
            anim.Play("Open");
        }

        if (hasMoved)
        {
            MoveButton.interactable = false;
            ActionUI.GetComponentsInChildren<Button>()[1].Select();
        }
        else { ActionUI.GetComponentInChildren<Button>().Select(); }
        ChangeCursorMode(5);
    }
    public void CloseUI()
    {
        if (ActionUI.TryGetComponent(out Animator anim))
        {
            anim.Play("Close");
        }
        else { ActionUI.gameObject.SetActive(false); }
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
        if(Char.TeamID != 0) { return; }
        hasMoved = true;
        actionActive = false;
        OpenUI();
        ChangeCursorMode(5);
    }

    private void SetupCombatUI(Entity entity)
    {
        int cursorMode = 0;
        Char = entity;
        hasMoved = false;
        MoveButton.interactable = true;

        UIOpen = false;
        actionActive = false;

        cancelDisp.SetActive(false);

        Debug.Log(entity);
        if (entity.TeamID == 0)
        {
            cursorMode = 5;
            OpenUI();
        }
        else
        {
            foreach (var item in abilityButton) { item.gameObject.SetActive(false); }
            abilityUI.SetActive(false);
            ActionUI.gameObject.SetActive(false);
        }

        ChangeCursorMode(cursorMode);
    }
    public void PositionUI(RectTransform rectTransform, Vector3 worldPos)
    {
        //Needs rework
        var screenPos = Camera.main.WorldToScreenPoint(worldPos);
        rectTransform.transform.position = new Vector2(screenPos.x, screenPos.y);
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
            //MAP MODE
            UIOpen = false;
            cancelDisp.SetActive(true);
        }
        else { cancelDisp.SetActive(false); }
        CombatEvents.current.SetCursorMode(mode, null);
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
}
