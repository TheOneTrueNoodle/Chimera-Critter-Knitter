using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Combat;

public class CombatHandler : MonoBehaviour
{
    [HideInInspector] public TurnHandler turnHandler;
    [HideInInspector] public MoveHandler moveHandler;
    [HideInInspector] public ActionHandler actionHandler;
    [HideInInspector] public UnitHandler unitHandler;
    [HideInInspector] public TileHandler tileHandler;

    [Header("World Text")]
    public Canvas UICanvas;
    public Transform damageTextParent;
    public GameObject damageTextPrefab;
    public Transform xpTextParent;
    public GameObject xpTextPrefab;

    //Unit Teams
    [Header("Unit Teams")]
    private Entity oscar;
    public List<Entity> playerTeam;
    public List<CombatAIController> enemyTeam;
    public List<CombatAIController> otherTeam;
    public List<CombatObstacle> obstacles;

    [Header("Start Combat Animation")]
    public StartCombatVisuals startCombatVisuals;
    public Animator startCombatAnim;

    //Dropped Loot after combat
    //List of dropped items

    private string currentCombatName;
    private float previousSong;

    private void Start()
    {
        CombatEvents.current.onStartCombat += StartCombat;
        CombatEvents.current.onHealAttempt += HealAttempt;
    }

    public void StartCombat(string combatName, List<CombatAIController> enemies, List<CombatAIController> others, List<CombatRoundEventData> RoundEvents, float BattleTheme)
    {
        SubscribeToCombatEvents();

        if (turnHandler == null) { turnHandler = new TurnHandler(); }
        if (moveHandler == null) { moveHandler = new MoveHandler(); }
        if (actionHandler == null){ actionHandler = new ActionHandler(); }
        if (unitHandler == null) { unitHandler = new UnitHandler(); }
        if (tileHandler == null) { tileHandler = new TileHandler(); }

        currentCombatName = combatName;

        var activeUnits = FindAllActiveUnits(enemies, others);
        turnHandler.StartCombat(activeUnits, RoundEvents);
        CombatEvents.current.AddLog("The battle begins!");

        //Animate the combat start ui
        StartCoroutine(StartCombatDisplay(combatName));

        //Music Time
        if (AreaManager.current.areaBools.ContainsKey("InCombat"))
        {
            AreaManager.current.areaBools["InCombat"] = true;
        }

        previousSong = AudioManager.instance.GetCurrentSong();
        Debug.Log(BattleTheme.ToString());
        AudioManager.instance.SetMusicSong(BattleTheme);
    }

    public void EndCombat()
    {
        Debug.Log("Combat over");
        UnsubscribeToCombatEvents();

        tileHandler.ClearUnitTiles(playerTeam, enemyTeam, otherTeam, obstacles);

        playerTeam.Clear();
        enemyTeam.Clear();
        otherTeam.Clear();

        turnHandler.EndCombat();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.victoryMusic, transform.position);
        AudioManager.instance.SetMusicSong(previousSong);
        CombatEvents.current.OpenVictoryUI();
    }

    public IEnumerator DelayedTurnEnd()
    {
        yield return new WaitForSeconds(0.5f);
        CombatEvents.current.TurnEnd();
    }

    public void TurnEnd()
    {
        UpdateMusic();
        tileHandler.ClearTiles();
        turnHandler.nextTurn();
    }

    private void UpdateMusic()
    {
        float percentageHealthMissing = ((oscar.activeStatsDir["MaxHP"].baseStatValue - oscar.activeStatsDir["MaxHP"].statValue) / oscar.activeStatsDir["MaxHP"].baseStatValue) * 100;
        AudioManager.instance.SetSongParameter("Intensity", percentageHealthMissing);
    }

    #region Combat Interactions
    public void MoveAttempt(Entity entity, List<OverlayTile> path)
    {
        StartCoroutine(moveHandler.MoveAlongPath(entity, path));
    }

    public void AttackAttempt(Entity attacker, Entity target)
    {
        StartCoroutine(Attack(attacker, target));
    }

    public IEnumerator Attack(Entity attacker, Entity target)
    {
        string newLog = "";
        int damage = actionHandler.Attack(attacker, target);

        //Animate Attack
        Rigidbody rb = attacker.gameObject.GetComponent<Rigidbody>();
        var rot = Quaternion.LookRotation((target.transform.position - attacker.transform.position), Vector3.up);
        float angle = Quaternion.Angle(rb.rotation, rot);

        while (angle > 0.1f)
        {
            rb.rotation = Quaternion.RotateTowards(attacker.transform.rotation, rot, 720f * Time.deltaTime);
            angle = Quaternion.Angle(rb.rotation, rot);
            yield return null;
        }

        if (attacker.anim != null)
        {
            if (attacker.heldWeapon != null)
            {
                attacker.anim.Play("Attack With Weapon");
            }
            else
            {
                attacker.anim.Play("Attack");
            }

            //Audio for attack
            if (attacker.heldWeapon != null && !attacker.heldWeapon.weaponAttackSFX.IsNull)
            {
                AudioManager.instance.PlayOneShot(attacker.heldWeapon.weaponAttackSFX, attacker.transform.position);
            }
            else if(!attacker.CharacterData.defaultAttackSFX.IsNull)
            {
                AudioManager.instance.PlayOneShot(attacker.CharacterData.defaultAttackSFX, attacker.transform.position);
            }


            else { Debug.LogError("NO ATTACK AUDIO ASSIGNED"); }
            yield return new WaitUntil(() => attacker.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > attacker.CharacterData.attackAnimationTimer);
        }

        bool tookDamage = false;
        if (damage > 0 && !target.isDead)
        {
            if (target.TryGetComponent(out CombatObstacle obstacle))
            {
                if (obstacle.Destructable)
                {
                    unitHandler.UnitSufferDamage(target, damage);
                    newLog = attacker.CharacterData.Name + " attacked " + target.CharacterData.Name + " and dealt " + damage + " " + attacker.AttackDamageType + " damage!";
                    DisplayDamageText(damage, target, attacker.AttackDamageType);
                    tookDamage = true;
                }
            }
            else
            {
                unitHandler.UnitSufferDamage(target, damage);
                DisplayDamageText(damage, target, attacker.AttackDamageType);
                newLog = attacker.CharacterData.Name + " attacked " + target.CharacterData.Name + " and dealt " + damage + " " + attacker.AttackDamageType + " damage!";
                tookDamage = true;
            }
        }
        else if(damage <= 0)
        {
            newLog = attacker.CharacterData.Name + " attacked " + target.CharacterData.Name + " but dealt no damage!";
            DisplayDamageText(0, target, attacker.AttackDamageType);
        }

        //Animate Taking Damage
        if (target.anim != null && tookDamage)
        {
            target.anim.Play("Take Damage");
            yield return new WaitUntil(() => target.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        //LOG RESULT OF THE ATTACK
        if (newLog != "")
        {
            CombatEvents.current.AddLog(newLog);
        }
        StartCoroutine(DelayedTurnEnd());
    }

    public void AbilityAttempt(Entity attacker, List<Entity> targets, AbilityData ability, OverlayTile abilityCenter, bool isItem)
    {
        StartCoroutine(Ability(attacker, targets, ability, abilityCenter, isItem));
    }

    public IEnumerator Ability(Entity attacker, List<Entity> targets, AbilityData ability, OverlayTile abilityCenter, bool isItem)
    {
        CombatEvents.current.AddLog(new string(attacker.CharacterData.Name + " uses " + ability.Name + "!"));

        List<Entity> affectedTargets = actionHandler.CalculateAbilityTargets(attacker, ability.abilityType, targets);
        Debug.Log(affectedTargets.Count);

        List<string> newLogs = new List<string>();
        for (int i = 0; i < affectedTargets.Count; i++)
        {
            newLogs.Add("");
        }

        //Animate Ability
        if (attacker.activeTile != abilityCenter)
        {
            //Rotate Towards Target
            Rigidbody rb = attacker.gameObject.GetComponent<Rigidbody>();
            var rot = Quaternion.LookRotation((abilityCenter.transform.position - attacker.transform.position), Vector3.up);
            float angle = Quaternion.Angle(rb.rotation, rot);

            while (angle > 0.1f)
            {
                rb.rotation = Quaternion.RotateTowards(attacker.transform.rotation, rot, 720f * Time.deltaTime);
                angle = Quaternion.Angle(rb.rotation, rot);
                yield return null;
            }
        }

        //Play Attacker Animation
        if (attacker.anim != null)
        {
            attacker.anim.Play(ability.AnimationName);
            yield return new WaitUntil(() => attacker.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        //Spawn and Play Animation Visuals
        if (ability.abilityVisual != null)
        {
            AbilityVisual visuals = Instantiate(ability.abilityVisual).GetComponent<AbilityVisual>();

            //Get middle of group of enemies
            var bound = new Bounds(affectedTargets[0].transform.position, Vector3.zero);
            for (int i = 1; i < affectedTargets.Count; i++)
            {
                bound.Encapsulate(affectedTargets[i].transform.position);
            }

            Vector3 targetPosition = bound.center;

            visuals.Setup(targetPosition, attacker.transform);
        }

        for(int i = 0; i < affectedTargets.Count; i++)
        {
            int abilityDamage = actionHandler.Ability(attacker, affectedTargets[i], ability);
            int physicalDamage = 0;

            if (ability.addMeleeAttack)
            {
                physicalDamage = actionHandler.Attack(attacker, affectedTargets[i]);
            }

            foreach (var effect in ability.effects)
            {
                if (effect.affectUser)
                    unitHandler.UnitSufferEffect(attacker, effect);
                else
                    unitHandler.UnitSufferEffect(affectedTargets[i], effect);
            }

            if (physicalDamage > 0 && !affectedTargets[i].isDead)
            {
                newLogs[i] = attacker.CharacterData.Name + " attacked " + affectedTargets[i].CharacterData.Name + " and dealt " + physicalDamage + " " + attacker.AttackDamageType + " damage!";
                unitHandler.UnitSufferDamage(affectedTargets[i], physicalDamage);
                DisplayDamageText(physicalDamage, affectedTargets[i], attacker.AttackDamageType);
            }
            else if(physicalDamage <= 0 && !affectedTargets[i].isDead)
            {
                DisplayDamageText(0, affectedTargets[i], attacker.AttackDamageType);
            }

            if (abilityDamage > 0 && !affectedTargets[i].isDead)
            {
                newLogs[i] = affectedTargets[i].CharacterData.Name + " suffers " + abilityDamage + " " + ability.damageType + " damage from " + ability.Name + "!";
                unitHandler.UnitSufferAbility(affectedTargets[i], ability, abilityDamage);
                DisplayDamageText(abilityDamage, affectedTargets[i], ability.damageType);
            }
            else if(abilityDamage <= 0 && !affectedTargets[i].isDead)
            {
                DisplayDamageText(0, affectedTargets[i], ability.damageType);
            }

            if(physicalDamage + abilityDamage == 0)
            {
                newLogs[i] = affectedTargets[i] + " suffered no damage from " + ability.Name + "!";
            }
        }

        //LOG RESULT OF THE ABILITY
        foreach (string newLog in newLogs)
        {
            if (newLog != "")
            {
                CombatEvents.current.AddLog(newLog);
            }
        }

        if (isItem)
        {
            var item = attacker.heldConsumableItem;
            attacker.GetComponent<HeldItem>().DropItem();

            InteractionManager.current.interactables.Remove(item.GetComponent<Interactable>());

            Destroy(item.gameObject);
        }

        StartCoroutine(DelayedTurnEnd());
    }
    public void HealAttempt(Entity entity, int value)
    {
        unitHandler.UnitHeals(entity, value);
    }

    public void UnitDeath(Entity target)
    {
        turnHandler.UnitDeath(target);

        //LOG UNIT DEATH
        CombatEvents.current.AddLog(new string(target.CharacterData.Name + " has died!"));

        //Find dropped items
        if(target.TeamID > 0)
        {
            int EXP = Random.Range(target.CharacterData.minExpDrop, target.CharacterData.maxExpDrop);
            EXP /= playerTeam.Count;
            foreach (Entity entity in playerTeam)
            {
                GameObject xpDisp = Instantiate(xpTextPrefab, xpTextParent);
                CombatEvents.current.AddLog(entity + " gained " + EXP + " exp!");

                xpDisp.GetComponent<XPText>().Setup(EXP);
                CombatEvents.current.GiveUnitEXP(entity, EXP);
            }
        }

        if (target.TeamID == 1)
        {
            enemyTeam.Remove(target.GetComponent<CombatAIController>());
        }
        else if (target.TeamID == 0)
        {
            playerTeam.Remove(target);
            if (playerTeam.Count <= 0)
            {
                //GAME ENDSSSSS!!!!!!
                CombatEvents.current.GameOver();
                Time.timeScale = 0;
                AudioManager.instance.SetMusicSong(9);
            }
        }

        if (enemyTeam.Count <= 0)
        {
            EndCombat();
        }
    }

    public void DisplayDamageText(int damage, Entity target, DamageTypes damageType)
    {
        string dmgText = damage.ToString();
        bool showImage = true;
        if (damage <= 0) 
        { 
            dmgText = "MISS";
            showImage = false;
        }
        GameObject dmgDisp = Instantiate(damageTextPrefab, damageTextParent);

        var screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

        Vector3 actualScreenPos = new Vector3(screenPos.x / UICanvas.scaleFactor, screenPos.y / UICanvas.scaleFactor, 0);

        dmgDisp.GetComponent<RectTransform>().anchoredPosition3D = actualScreenPos;

        dmgDisp.GetComponent<DamageText>().Setup(dmgText, damageType, showImage);
    }
    #endregion

    #region Tile Functions

    public void TileColor(Entity entity, Color color, List<OverlayTile> tiles, bool hideActiveTile)
    {
        tileHandler.ColorTiles(entity, color, tiles, hideActiveTile);
    }

    public void TileClearSpecific(List<OverlayTile> tiles)
    {
        tileHandler.ClearSpecificTiles(tiles);
    }

    public void TilePositionEntity(Entity entity, OverlayTile tile)
    {
        tileHandler.PositionCharacterOnTile(entity, tile);
    }

    #endregion

    #region Initialization
    private List<Entity> FindAllActiveUnits(List<CombatAIController> enemies, List<CombatAIController> others)
    {
        var ReturnUnits = new List<Entity>();

        var UnitsUnsorted = FindObjectsOfType<Entity>();
        playerTeam = new List<Entity>();
        enemyTeam = enemies;
        otherTeam = others;

        oscar = FindObjectOfType<PlayerMovement>().GetComponent<Entity>();

        foreach (var item in UnitsUnsorted)
        {
            if (item.TeamID == 0)
            {
                Debug.Log("Found player");
                item.Initialize();
                ReturnUnits.Add(item);
                playerTeam.Add(item);
            }
            else if (item.TeamID == -1)
            {
                item.Initialize();
                obstacles.Add(item.GetComponent<CombatObstacle>());
            }
        }

        foreach (CombatAIController enemy in enemyTeam)
        {
            enemy.Initialize();
            enemy.InitializeCombat(playerTeam);
            ReturnUnits.Add(enemy);
        }

        /*
        foreach (CombatAIController other in otherTeam)
        {
            other.InitializeCombat(playerTeam);
            ReturnUnits.Add(other);
        }*/

        return ReturnUnits;
    }

    public void UnitStartingEffects(Entity entity, List<EquipmentStatChanges> equipmentStatChanges, List<ScriptableEffect> additionalEffects)
    {
        if (equipmentStatChanges != null)
        {
            for (int i = 0; i < equipmentStatChanges.Count; i++)
            {
                string statChangeName = equipmentStatChanges[i].statKey.ToString() + " " + i.ToString();

                var statToEffect = entity.activeStatsDir[System.Enum.GetName(typeof(StatsList), equipmentStatChanges[i].statKey)];
                statToEffect.statMods.Add(new StatModifier(equipmentStatChanges[i].statKey, equipmentStatChanges[i].value, -1, equipmentStatChanges[i].op, statChangeName, DamageTypes.Healing));

                statToEffect.ApplyStatMod(entity);
            }
        }
        if (additionalEffects != null)
        {
            foreach (ScriptableEffect effect in additionalEffects)
            {
                unitHandler.UnitSufferEffect(entity, effect);
            }
        }
    }

    public void UnitStartingAbilities(Entity entity, List<AbilityData> abilities)
    {
        entity.activeAbilities = entity.CharacterData.SetAbilities(abilities);
    }

    public void AddUnitToCombat(Entity entity)
    {
        turnHandler.AddUnitToTurnOrder(entity);

        if(entity.GetComponent<CombatAIController>())
        {
            entity.GetComponent<CombatAIController>().InitializeCombat(playerTeam);
        }
        entity.Initialize(); 
        if (entity.TeamID == 0)
        {
            playerTeam.Add(entity);
        }
        else if (entity.TeamID == 1)
        {
            
            enemyTeam.Add(entity.GetComponent<CombatAIController>());
        }
        else
        {
            otherTeam.Add(entity.GetComponent<CombatAIController>());
        }
    }
    
    private IEnumerator StartCombatDisplay(string CombatName)
    {
        startCombatVisuals.gameObject.SetActive(true);
        startCombatAnim.gameObject.SetActive(true);

        if (startCombatVisuals != null)
        {
            startCombatVisuals.ChangeImages(CombatName);
            startCombatAnim.Play("New Start Combat");
            yield return new WaitUntil(() => startCombatAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        startCombatVisuals.gameObject.SetActive(false);
        startCombatAnim.gameObject.SetActive(false);
        CombatEvents.current.StartCombatSetup(currentCombatName);
        turnHandler.StartFirstTurn();
    }
    #endregion

    private void SubscribeToCombatEvents()
    {
        CombatEvents.current.onUnitDeath += UnitDeath;
        CombatEvents.current.onUnitStartingEffects += UnitStartingEffects;
        CombatEvents.current.onUnitStartingAbilities += UnitStartingAbilities;
        CombatEvents.current.onMoveAttempt += MoveAttempt;
        CombatEvents.current.onAttackAttempt += AttackAttempt;
        CombatEvents.current.onAbilityAttempt += AbilityAttempt;
        CombatEvents.current.onDamageDealt += DisplayDamageText;

        CombatEvents.current.onTileColor += TileColor;
        CombatEvents.current.onTileClearSpecific += TileClearSpecific;
        CombatEvents.current.onPositionEntity += TilePositionEntity;

        CombatEvents.current.onTurnEnd += TurnEnd;
    }
    private void UnsubscribeToCombatEvents()
    {
        CombatEvents.current.onUnitDeath -= UnitDeath;
        CombatEvents.current.onUnitStartingEffects -= UnitStartingEffects;
        CombatEvents.current.onUnitStartingAbilities -= UnitStartingAbilities;
        CombatEvents.current.onMoveAttempt -= MoveAttempt;
        CombatEvents.current.onAttackAttempt -= AttackAttempt;
        CombatEvents.current.onAbilityAttempt -= AbilityAttempt;
        CombatEvents.current.onDamageDealt -= DisplayDamageText;

        CombatEvents.current.onTileColor -= TileColor;
        CombatEvents.current.onTileClearSpecific -= TileClearSpecific;
        CombatEvents.current.onPositionEntity -= TilePositionEntity;

        CombatEvents.current.onTurnEnd -= TurnEnd;
    }
}