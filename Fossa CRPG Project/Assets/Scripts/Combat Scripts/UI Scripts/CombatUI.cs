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
    [SerializeField] private GameObject OscarHPDisp;
    [SerializeField] private RectTransform abilityUI;
    private List<AbilityButton> abilityButton = new List<AbilityButton>();

    [Header("Use Item UI")]
    [SerializeField] private GameObject ItemButtonUI;
    [SerializeField] private TMP_Text itemNameText;

    [Header("Turn Order Display")]
    [SerializeField] private GameObject turnIconPrefab;
    [SerializeField] private Transform turnIconParent;
    private List<TurnOrderIcon> activeTurnIcons;

    [Header("End Combat UI")]
    [SerializeField] private GameObject endCombatUI;
    [SerializeField] private Animator gameOverAnim;

    private bool UIOpen;
    private bool abilityUIOpen;
    private bool gamePaused;
    private bool actionActive;
    private bool hasMoved;
    private bool mapOpen;

    private string currentCombatName;

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
        CombatEvents.current.onUnpauseGame += UnpauseGame;
        CombatEvents.current.onGameOver += GameOver;

        cursor = FindObjectOfType<CursorController>();
        foreach (var item in GetComponentsInChildren<AbilityButton>(true))
        {
            abilityButton.Add(item);
        }
    }

    private void Update()
    {
        if (!started) { return; }
        UpdateInformationUI();
        GetActionInput();
    }

    private void GetActionInput()
    {
        if(Char.TeamID != 0)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                //Pause Menu Inputs!!!
                gamePaused = true;
                CombatEvents.current.PauseGame();
            }
                
            return; 
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (mapOpen)
            {
                //Close map
                CancelInput();
            }
            else if (!actionActive && !abilityUIOpen)
            {
                //Pause Menu Inputs!!!
                gamePaused = true;
                CombatEvents.current.PauseGame();
            }
            else if (!UIOpen)
            {
                OpenUI();
            }
            else if (UIOpen)
            {
                CancelInput();
            }
        }
    }
    public void CancelInput()
    {
        if (abilityUIOpen && actionActive != true)
        {
            CloseAbilityUI();
            ExamineUI.gameObject.SetActive(false);
            OpenUI();
        }
        else if (actionActive)
        {
            CloseAbilityUI();
            ExamineUI.gameObject.SetActive(false);
            OpenUI();
            actionActive = false;
        }
        else if (cursor.cursorMode == 6)
        {
            ExamineUI.gameObject.SetActive(false);
            ChangeCursorMode(1);
        }
        else if (cursor.cursorMode == 1)
        {
            ExamineUI.gameObject.SetActive(false);
            OpenUI();
        }
    }
    public void OpenUI()
    {
        //Default mode

        UIOpen = true;
        //Display Action UI
        ActionUI.gameObject.SetActive(true);
        if (Char.heldConsumableItem != null)
        {
            ItemButtonUI.SetActive(true);
            itemNameText.text = Char.heldConsumableItem.itemName;
        }
        else
        {
            ItemButtonUI.SetActive(false);
        }

        if (ActionUI.TryGetComponent(out Animator anim))
        {
            anim.Play("Open");
        }

        ChangeCursorMode(5);
    }
    public void CloseUI()
    {
        if (ActionUI.TryGetComponent(out Animator anim))
        {
            anim.Play("Close");
        }
        else { ActionUI.gameObject.SetActive(false); }

        UIOpen = false;
    }

    public void AbilityButtonCall()
    {
        if (abilityUIOpen)
        {
            CloseAbilityUI();
        }
        else
        {
            OpenAbilityUI();
        }
    }

    public void OpenAbilityUI()
    {
        abilityUIOpen = true;

        OscarHPDisp.gameObject.SetActive(false);
        abilityUI.gameObject.SetActive(true);
        ActionUI.gameObject.SetActive(false);

        foreach (AbilityButton button in abilityButton)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < Char.activeAbilities.Count; i++)
        {
            abilityButton[i].gameObject.SetActive(true);
            abilityButton[i].SetupButton(Char.activeAbilities[i], i);
            if(Char.activeStatsDir["MaxSP"].statValue < Char.activeAbilities[i].abilityCost) { abilityButton[i].GetComponent<Button>().interactable = false; }
            else { abilityButton[i].GetComponent<Button>().interactable = true; }
        }

        if (Char.activeAbilities.Count > 0)
        {
            abilityButton[0].GetComponent<Button>().Select();
        }
    }

    public void CloseAbilityUI()
    {
        foreach (var item in abilityButton)
        {
            item.gameObject.SetActive(false);
        }

        OscarHPDisp.gameObject.SetActive(true);
        abilityUI.gameObject.SetActive(false);
        ActionUI.gameObject.SetActive(true);

        abilityUIOpen = false;
    }
    public void UseConsumableItem()
    {
        actionActive = true;
        cancelDisp.SetActive(true);
        foreach (var item in abilityButton)
        {
            item.gameObject.SetActive(false);
        }

        CombatEvents.current.SetCursorMode(4, Char.heldConsumableItem.itemAbility, true);
    }

    public void CallAbility(int ID)
    {
        actionActive = true;
        cancelDisp.SetActive(true);
        foreach (var item in abilityButton)
        {
            item.gameObject.SetActive(false);
        }
        CombatEvents.current.SetCursorMode(4, Char.activeAbilities[ID], false);
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
        OscarHPDisp.gameObject.SetActive(true);
        abilityUI.gameObject.SetActive(false);

        UIOpen = false;
        actionActive = false;

        cancelDisp.SetActive(false);

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

    public void UpdateInformationUI()
    {
        if (!started) 
        { return; }

        if(Char == null)
        {
            //Char is null so we must find Oscar
            Char = FindObjectOfType<PlayerMovement>().GetComponent<Entity>();
        }

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
            mapOpen = true;
        }
        else { cancelDisp.SetActive(false); }
        CombatEvents.current.SetCursorMode(mode, null, false);
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
    private void StartCombat(string combatName)
    {
        currentCombatName = combatName;
        started = true;
        UIParent.SetActive(true);
        endCombatUI.SetActive(false);
        endCombatUI.SetActive(false);
    }
    private void OpenVictoryUI()
    {
        endCombatUI.SetActive(true);
        endCombatUI.GetComponent<Animator>().Play("Open");
    }
    public void EndCombat( )
    {
        started = false;
        UIParent.SetActive(false);
        CombatEvents.current.EndCombat(currentCombatName);
    }

    public void UnpauseGame()
    {
        gamePaused = false;
    }

    public void GameOver()
    {
        //Animate Game Over UI


        //Change to Game Over Music

        gameOverAnim.Play("Game Over");
    }
}
