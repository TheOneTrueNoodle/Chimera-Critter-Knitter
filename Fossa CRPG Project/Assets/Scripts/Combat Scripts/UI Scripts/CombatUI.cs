using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    [Header("Unit Information Display")]
    public UnitInfoUI currentUnitUI;
    public UnitInfoUI selectedUnitUI;
    public ExamineUnitUI ExamineUI;

    private Entity previousPlayerChar;
    private Entity Char;
    private bool started;

    [Header("Functionality")]
    [SerializeField] private GameObject UIParent;
    [SerializeField] private RectTransform ActionUI;
    [SerializeField] private Button MoveButton;
    [SerializeField] private GameObject cancelDisp;

    [Header("Ability UI")]
    [SerializeField] private RectTransform abilityUI;
    [SerializeField] private GameObject abilityButtonPrefab;
    [SerializeField] private RectTransform abilityButtonParent;
    [SerializeField] private float abilityButtonOffset = 10f;
    private List<AbilityButton> abilityButton = new List<AbilityButton>();

    [Header("End Turn UI")]
    [SerializeField] private Button EndTurnButton;
    [SerializeField] private Image RingFill;
    private float EndTurnFill;

    [Header("Turn Order Display")]
    [SerializeField] private GameObject turnIconPrefab;
    [SerializeField] private Transform turnIconParent;
    private List<TurnOrderIcon> activeTurnIcons;

    [Header("End Combat UI")]
    [SerializeField] private GameObject endCombatUI;

    private bool UIOpen;
    private bool actionActive;
    private bool hasMoved;

    private CursorController cursor;

    private void Start()
    {
        CombatEvents.current.onNewTurn += SetupCombatUI;
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onTurnOrderDisplay += NewRoundTurnOrder;
        CombatEvents.current.onOpenVictoryUI += OpenVictoryUI;
        CombatEvents.current.onActionComplete += ActionComplete;
        CombatEvents.current.onGetSelectedTile += SetupExamineUI;
        CombatEvents.current.onGiveUnitEXP += GiveUnitEXP;

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
                CancelInput();
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
    public void CancelInput()
    {
        if (abilityUI.gameObject.activeInHierarchy && actionActive != true)
        {
            CloseAbilityUI();
            OpenUI();
            ChangeCursorMode(5);
        }
        else if (actionActive)
        {
            CloseAbilityUI();
            ExamineUI.gameObject.SetActive(false);
            OpenUI();
            actionActive = false;
        }

        ExamineUI.gameObject.SetActive(false);
    }
    public void OpenUI()
    {
        UIOpen = true;
        //Display Action UI
        ActionUI.gameObject.SetActive(true);

        Vector3 position = Char.gameObject.transform.position;
        if(Char.UITarget != null) { position = Char.UITarget.position; }
        else { Debug.LogError("No assigned UITarget"); }

        PositionUI(ActionUI, position);
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
        abilityUI.gameObject.SetActive(true);
        ActionUI.gameObject.SetActive(false);
        abilityUI.transform.position = new Vector2(ActionUI.transform.position.x + abilityUI.sizeDelta.x, ActionUI.transform.position.y);

        foreach (var button in abilityButton)
        {
            Destroy(button);
        }

        abilityButton.Clear();

        for (int i = 0; i < Char.activeAbilities.Count; i++)
        {
            AbilityButton newAbilityButton = Instantiate(abilityButtonPrefab, abilityButtonParent).GetComponent<AbilityButton>();
            newAbilityButton.SetupButton(Char.activeAbilities[i], i);
            abilityButton.Add(newAbilityButton);
        }

        if (Char.activeAbilities.Count > 0)
        {
            abilityButton[0].GetComponent<Button>().Select();
        }
    }

    public void CloseAbilityUI()
    {
        foreach (var item in abilityButton) { Destroy(item.gameObject); }
        abilityButton.Clear();
        abilityUI.gameObject.SetActive(false);
        ActionUI.gameObject.SetActive(true);
    }

    public void CallAbility(int ID)
    {
        actionActive = true;
        cancelDisp.SetActive(true);
        foreach (var item in abilityButton) { Destroy(item.gameObject); }
        abilityButton.Clear();
        abilityUI.gameObject.SetActive(false);
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
        Debug.Log(Char);
        if (Char != null)
        {
            if(Char.TeamID == 0) { previousPlayerChar = Char; }
            var previousTurnIcon = activeTurnIcons.First();
            activeTurnIcons.RemoveAt(0);
            Destroy(previousTurnIcon.gameObject);
        }

        if (entity.TeamID != 0)
        {
            selectedUnitUI.gameObject.SetActive(true);
            selectedUnitUI.UpdateUI(entity);
        }

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
            abilityUI.gameObject.SetActive(false);
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
        if (!started) 
        { return; }
        if (Char != null)
        {
            if (Char.TeamID == 0)
            {
                currentUnitUI.UpdateUI(Char);
            }
            else
            {
                selectedUnitUI.UpdateUI(Char);
            }

            if (previousPlayerChar != null)
            {
                currentUnitUI.UpdateUI(previousPlayerChar);
            }


            var selectedTile = cursor.currentTile;
            if (selectedTile != null && Char.TeamID == 0)
            {
                if (selectedTile.activeCharacter != null && selectedTile.activeCharacter != Char)
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
                        selectedUnitUI.CloseUI();
                    }
                }
            }
        }
    }
    public void ChangeCursorMode(int mode)
    {
        if (mode == 2 || mode == 3 || mode == 4 || mode == 6)
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
    private void NewRoundTurnOrder(List<Entity> allUnits)
    {
        if(activeTurnIcons == null) { activeTurnIcons = new List<TurnOrderIcon>(); }
        Char = null;
        
        foreach (TurnOrderIcon turnIcon in activeTurnIcons)
        {
            Destroy(turnIcon.gameObject);
        }
        activeTurnIcons.Clear();
        Debug.Log(allUnits.Count);
        foreach (Entity unit in allUnits)
        {
            TurnOrderIcon newTurnOrderIcon = Instantiate(turnIconPrefab, turnIconParent).GetComponent<TurnOrderIcon>();
            activeTurnIcons.Add(newTurnOrderIcon);
            newTurnOrderIcon.Setup(unit);
        }
    }
    private void SetupExamineUI(OverlayTile overlayTile)
    {
        UIOpen = true;
        ExamineUI.gameObject.SetActive(true);
        ExamineUI.AssignEntity(overlayTile);
        ChangeCursorMode(6);
    }
    private void GiveUnitEXP(Entity unit, int xp)
    {

        if (currentUnitUI.currentChar = unit)
        {
            unit.IncreaseEXP(currentUnitUI.EXPBar, xp);
        }
        else
        {
            unit.IncreaseEXP(null, xp);
        }
    }
    private void StartCombat()
    {
        started = true;
        UIParent.SetActive(true);
        endCombatUI.SetActive(false);
    }
    private void OpenVictoryUI()
    {
        endCombatUI.SetActive(true);
        endCombatUI.GetComponent<Animator>().Play("Open");
    }
    public void EndCombat()
    {
        started = false;
        UIParent.SetActive(false);
        CombatEvents.current.EndCombat();
    }
}
