using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Combat;

public class CursorController : MonoBehaviour
{
    public bool inCombat;

    public float unitSpeed;
    public Entity activeCharacter;

    private TileFunctions tileFunctions;
    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private ArrowTranslator arrowTranslator;
    private ShapeParser shapeParser;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    //EnemyTurn = 0, Default = 1, Movement = 2, Attack = 3, Ability = 4, UI = 5
    [HideInInspector] public int cursorMode;
    public bool isMoving = false;

    [HideInInspector] public OverlayTile currentTile;

    //Booleans for turn actions
    private bool hasMoved;
    private bool hasAttacked;
    private bool hasCastAbility;
    private AbilityData currentAbility;
    private List<OverlayTile> abilityArea = new List<OverlayTile>();

    private float moveCursorDelay = 0.08f;
    private float moveCursorDelayTimer;

    private bool UIMode;
    private OverlayTile UISelectedTile;

    private void Start()
    {
        CombatEvents.current.onStartCombat += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;

        CombatEvents.current.onNewTurn += NewTurn;
        CombatEvents.current.onSetCursorMode += SetCursorMode;
        CombatEvents.current.onTileClicked += TileClicked;
        CombatEvents.current.onActionComplete += FinishedMovement;

        tileFunctions = new TileFunctions();
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
        arrowTranslator = new ArrowTranslator();
        shapeParser = new ShapeParser();
    }

    private void LateUpdate()
    {
        if (!inCombat) { return; }

        OverlayTile overlayTile = null;

        //Handle Cursor Function
        switch (cursorMode)
        {
            case 5:
                //UI Mode
                break;
            case 4:
                //Ability Mode
                overlayTile = GetCursorPosition();
                if (overlayTile != null) 
                { 
                    Ability(overlayTile);
                }
                break;
            case 3:
                //Attack Mode
                overlayTile = GetCursorPosition();
                break;
            case 2:
                //Move Mode
                overlayTile = GetCursorPosition();
                if (overlayTile != null) 
                { 
                    Movement(overlayTile);
                }
                break;
            case 1:
                //Default Mode
                overlayTile = GetCursorPosition();
                break;
            default:
                //Enemy Turn 
                break;
        }

        if (Input.GetButtonDown("Submit") && overlayTile != null) { TileClicked(overlayTile); }
    }

    public void TileClicked(OverlayTile overlayTile)
    {
        if (!inCombat) { return; }
        //Handle Cursor Click Function
        switch (cursorMode)
        {
            case 5:
                //UI Mode
                break;
            case 4:
                //Ability Mode
                if (overlayTile != null) { AbilityClick(overlayTile); }
                break;
            case 3:
                //Attack Mode
                if (overlayTile != null) { AttackClick(overlayTile); }
                break;
            case 2:
                //Move Mode
                if (overlayTile != null) { MovementClick(overlayTile); }
                break;
            case 1:
                //Default Mode
                break;
            default:
                //Enemy Turn
                break;
        }
    }

    #region CombatModes
    // DEFENDING
    public void Defend()
    {
        activeCharacter.isDefending = true;
        CombatEvents.current.TurnEnd();
    }

    // ABILITY
    public void Ability(OverlayTile overlayTile)
    {
        if (hasCastAbility) { return; }

        if (inRangeTiles.Contains(overlayTile))
        {
            if (abilityArea.Count != 0)
            {
                foreach (var item in abilityArea)
                {
                    item.HideTile();
                }
            }
            abilityArea.Clear();

            abilityArea = shapeParser.GetAbilityTileLocations(overlayTile, currentAbility.abilityShape, activeCharacter.activeTile.grid2DLocation);

            foreach (var item in inRangeTiles)
            {
                item.ShowTile();
            }
            CombatEvents.current.TileColor(activeCharacter, Color.blue, abilityArea, false);
        }
        else
        {
            if (abilityArea.Count != 0)
            {
                foreach (var item in abilityArea)
                {
                    item.HideTile();
                }
            }
            abilityArea.Clear();
            foreach (var item in inRangeTiles)
            {
                item.ShowTile();
            }
        }
    }

    public void AbilityClick(OverlayTile overlayTile)
    {
        if (hasCastAbility) { return; }

        if (inRangeTiles.Contains(overlayTile))
        {
            CastAbility(abilityArea);
        }
    }

    private void CastAbility(List<OverlayTile> abilityArea)
    {
        var inRangeCharacters = new List<Entity>();

        foreach (var tile in abilityArea)
        {
            var targetCharacter = tile.activeCharacter;
            if (targetCharacter != null)
            {
                inRangeCharacters.Add(targetCharacter);
            }
            tile.HideTile();
        }
        if (currentAbility.requiresTarget && inRangeCharacters.Count > 0 || !currentAbility.requiresTarget)
        {
            ClearRangeTiles();

            if (currentAbility.costType == AbilityData.CostTypes.HP && currentAbility.abilityCost <= (int)activeCharacter.activeStatsDir["MaxHP"].statValue)
            {
                Debug.Log("Casting " + currentAbility.Name);
                hasCastAbility = true;
                activeCharacter.activeStatsDir["MaxHP"].statValue -= currentAbility.abilityCost;
                activeCharacter.updateHealthBar();
            }
            else if (currentAbility.costType == AbilityData.CostTypes.SP && currentAbility.abilityCost <= (int)activeCharacter.activeStatsDir["MaxSP"].statValue)
            {
                Debug.Log("Casting " + currentAbility.Name);
                hasCastAbility = true;
                activeCharacter.activeStatsDir["MaxSP"].statValue -= currentAbility.abilityCost;
            }

            if (hasCastAbility)
            {
                CombatEvents.current.AbilityAttempt(activeCharacter, inRangeCharacters, currentAbility);
            }
        }
    }

    // ATTACKING
    public void AttackClick(OverlayTile overlayTile)
    {
        if (inRangeTiles.Contains(overlayTile) && overlayTile.isBlocked == true && !hasAttacked)
        {
            //Attack!!!
            if (overlayTile.activeCharacter.TeamID != activeCharacter.TeamID)
            {
                hasAttacked = true;

                CombatEvents.current.AttackAttempt(activeCharacter, overlayTile.activeCharacter);
            }
        }
    }

    // MOVING
    public void Movement(OverlayTile overlayTile)
    {
        if (hasMoved) { return; }

        if (inRangeTiles.Contains(overlayTile) && !isMoving)
        {
            path = pathFinder.FindPath(activeCharacter.activeTile, overlayTile, inRangeTiles);

            foreach (var item in inRangeTiles)
            {
                item.SetArrowSprite(ArrowTranslator.ArrowDirection.None);
            }

            for (int i = 0; i < path.Count; i++)
            {
                var previousTile = i > 0 ? path[i - 1] : activeCharacter.activeTile;
                var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                var arrowDir = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                path[i].SetArrowSprite(arrowDir);
            }
        }
    }

    public void MovementClick(OverlayTile overlayTile)
    {
        if (hasMoved || isMoving) { return; }

        if (path.Count > 0 && inRangeTiles.Contains(overlayTile) && activeCharacter != null)
        {
            isMoving = true;
            CombatEvents.current.MoveAttempt(activeCharacter, path);
        }

    }

    public void FinishedMovement()
    {
        isMoving = false;
        hasMoved = true;
    }

    #endregion
    #region functions
    private OverlayTile GetCursorPosition()
    {
        OverlayTile overlayTile = GetMovementInput();
        if (overlayTile != null)
        {
            transform.position = overlayTile.gameObject.transform.position;
            gameObject.GetComponentInChildren<Canvas>().sortingOrder = overlayTile.GetComponentInChildren<Canvas>().sortingOrder + 1;
        }

        return overlayTile;
    }

    private void GetInRangeTiles(int range, bool hideOccupiedTiles, bool includeOrigin)
    {
        if (cursorMode == 2)
        {
            inRangeTiles = rangeFinder.GetMovementTilesInRange(activeCharacter, range, hideOccupiedTiles, includeOrigin);
        }
        else
        {
            List<OverlayTile> startTiles = new List<OverlayTile>();
            startTiles.Add(activeCharacter.activeTile);
            /*
            foreach (EntitySubTile subTile in activeCharacter.subTileSpaces)
            {
                startTiles.Add(subTile.subTile);
            }
            */

            inRangeTiles = rangeFinder.GetTilesInRange(startTiles, range, hideOccupiedTiles, includeOrigin);
        }

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
        activeCharacter.activeTile.HideTile();
    }

    private void ClearRangeTiles()
    {
        if (inRangeTiles.Count == 0) { return; }
        CombatEvents.current.TileClearSpecific(inRangeTiles);

        inRangeTiles.Clear();
    }

    public RaycastHit? GetFocusedOnTile()
    {
        int layer_mask = LayerMask.GetMask("OverlayTiles");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
        {
            return hit;
        }
        else
        {
            return null;
        }
    }

    private OverlayTile GetMovementInput()
    {
        bool usingMouse = false;
        moveCursorDelayTimer -= Time.deltaTime;

        //Get ControlMode
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            usingMouse = true;
        }

        switch (usingMouse)
        {
            case true:
                var focusedTileHit = GetFocusedOnTile();
                if (focusedTileHit.HasValue)
                {
                    currentTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
                    return focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
                }
                break;
            case false:
                if (moveCursorDelayTimer > 0) { return null; }

                Vector3 _input;
                _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                if(_input == Vector3.zero && currentTile != null) { return currentTile; }

                var overlayTile = tileFunctions.GetSingleFocusedOnTile(new Vector3(gameObject.transform.position.x + _input.x, gameObject.transform.position.y + 10f, gameObject.transform.position.z + _input.z), false);
                if (overlayTile != null)
                {
                    currentTile = overlayTile;
                    moveCursorDelayTimer = moveCursorDelay;
                    return overlayTile;
                }
                break;
        }

        return null;
    }
    #endregion
    #region Event Calls
    private void NewTurn(Entity activeChar)
    {
        cursorMode = 1;
        activeCharacter = activeChar;
        activeChar.StartTurn();

        ClearRangeTiles();

        hasMoved = false;
        hasAttacked = false;
        hasCastAbility = false;
    }

    private void SetCursorMode(int mode, AbilityData abilityData)
    {
        Debug.Log("Changed Cursor Mode to " + mode);
        ClearRangeTiles();
        if (abilityArea.Count > 0)
        {
            foreach (OverlayTile tile in abilityArea)
            {
                tile.HideTile();
            }
        }
        cursorMode = mode;

        //Mode Startup
        switch (cursorMode)
        {
            case 5:
                //UI Startup
                if(currentTile != null && !UIMode)
                {
                    UISelectedTile = currentTile;
                    CombatEvents.current.GetSelectedTile(currentTile);
                    UIMode = true;
                }
                else if(UISelectedTile != null)
                {
                    transform.position = UISelectedTile.gameObject.transform.position;
                    gameObject.GetComponentInChildren<Canvas>().sortingOrder = UISelectedTile.GetComponentInChildren<Canvas>().sortingOrder + 1;
                }
                break;
            case 4:
                //Ability Startup
                if (abilityData != null)
                {
                    currentAbility = abilityData;
                    GetInRangeTiles(currentAbility.range, false, currentAbility.includeCenter);
                    CombatEvents.current.TileColor(activeCharacter, Color.white, inRangeTiles, false);
                }
                break;
            case 3:
                //Attack Startup
                GetInRangeTiles(activeCharacter.WeaponRange, false, false);
                CombatEvents.current.TileColor(activeCharacter, Color.red, inRangeTiles, false);
                break;
            case 2:
                //Move Startup
                if (hasMoved) { return; }
                GetInRangeTiles(activeCharacter.CharacterData.characterClass.MovementSpeed, true, false);
                CombatEvents.current.TileColor(activeCharacter, Color.white, inRangeTiles, false);
                break;
            case 1:
                //Default Startup
                UIMode = false;
                break;
            default:
                //Enemy Turn
                UIMode = false;
                break;
        }
    }

    public void StartCombat()
    {
        inCombat = true;
    }
    public void EndCombat()
    {
        inCombat = false;
    }
    #endregion
}

