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

    [Header("Damage Text")]
    public Transform damageTextParent;
    public GameObject damageTextPrefab;
    //Unit Teams
    [Header("Unit Teams")]
    private Entity oscar;
    public List<Entity> playerTeam;
    public List<CombatAIController> enemyTeam;
    public List<CombatAIController> otherTeam;
    public List<CombatObstacle> obstacles;

    [Header("Start Combat Animation")]
    public Animator startCombatAnim;
    //Dropped Loot after combat
    //List of dropped items
    public int totalDroppedExp;

    private float previousSong;

    private void Start()
    {
        CombatEvents.current.onStartCombat += StartCombat;

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

    public void StartCombat(List<CombatAIController> enemies, List<CombatAIController> others, List<CombatRoundEventData> RoundEvents, float BattleTheme)
    {
        if (turnHandler == null) { turnHandler = new TurnHandler(); }
        if (moveHandler == null) { moveHandler = new MoveHandler(); }
        if (actionHandler == null){ actionHandler = new ActionHandler(); }
        if (unitHandler == null) { unitHandler = new UnitHandler(); }
        if (tileHandler == null) { tileHandler = new TileHandler(); }

        var activeUnits = FindAllActiveUnits(enemies, others);
        turnHandler.StartCombat(activeUnits, RoundEvents);

        totalDroppedExp = 0;

        //Animate the combat start ui
        StartCoroutine(StartCombatDisplay());

        //Music Time
        previousSong = AudioManager.instance.GetCurrentSong();
        AudioManager.instance.SetMusicSong(BattleTheme);
    }

    public void EndCombat()
    {
        Debug.Log("Combat over");
        Debug.Log("Player Number: " + playerTeam.Count);
        Debug.Log("EXP: " + totalDroppedExp);
        if (totalDroppedExp > 0)
        {
            totalDroppedExp /= playerTeam.Count;
            foreach (Entity entity in playerTeam)
            {
                entity.IncreaseEXP(totalDroppedExp);
            }
        }

        playerTeam.Clear();
        enemyTeam.Clear();
        otherTeam.Clear();

        turnHandler.EndCombat();
        AudioManager.instance.SetMusicSong(previousSong);
        CombatEvents.current.EndCombat();
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
        Debug.Log("Moving");
        StartCoroutine(moveHandler.MoveAlongPath(entity, path));
    }

    public void AttackAttempt(Entity attacker, Entity target)
    {
        StartCoroutine(Attack(attacker, target));
    }

    public IEnumerator Attack(Entity attacker, Entity target)
    {
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
            attacker.anim.Play("Attack");
            yield return new WaitUntil(() => attacker.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        bool tookDamage = false;
        if (damage > 0 && !target.isDead)
        {
            if (target.TryGetComponent(out CombatObstacle obstacle))
            {
                if (obstacle.Destructable)
                {
                    unitHandler.UnitSufferDamage(target, damage);
                    DisplayDamageText(damage, target, attacker.AttackDamageType);
                    tookDamage = true;
                }
            }
            else
            {
                unitHandler.UnitSufferDamage(target, damage);
                DisplayDamageText(damage, target, attacker.AttackDamageType);
                tookDamage = true;
            }
        }

        //Animate Taking Damage
        if (target.anim != null && tookDamage)
        {
            target.anim.Play("Take Damage");
            yield return new WaitUntil(() => target.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        StartCoroutine(DelayedTurnEnd());
    }

    public void AbilityAttempt(Entity attacker, List<Entity> targets, AbilityData ability, Vector3 abilityCenter)
    {
        StartCoroutine(Ability(attacker, targets, ability, abilityCenter));
    }

    public IEnumerator Ability(Entity attacker, List<Entity> targets, AbilityData ability, Vector3 abilityCenter)
    {
        List<Entity> affectedTargets = actionHandler.CalculateAbilityTargets(attacker, ability.abilityType, targets);
        Debug.Log(affectedTargets.Count);

        //Animate Ability
        Rigidbody rb = attacker.gameObject.GetComponent<Rigidbody>();
        var rot = Quaternion.LookRotation((abilityCenter - attacker.transform.position), Vector3.up);
        float angle = Quaternion.Angle(rb.rotation, rot);

        while (angle > 0.1f)
        {
            rb.rotation = Quaternion.RotateTowards(attacker.transform.rotation, rot, 720f * Time.deltaTime);
            angle = Quaternion.Angle(rb.rotation, rot);
            yield return null;
        }

        if (attacker.anim != null)
        {
            attacker.anim.Play(ability.AnimationName);
            yield return new WaitUntil(() => attacker.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        foreach (Entity target in affectedTargets)
        {
            int abilityDamage = actionHandler.Ability(attacker, target, ability);
            int physicalDamage = 0;

            if (ability.addMeleeAttack)
            {
                physicalDamage = actionHandler.Attack(attacker, target);
            }

            foreach (var effect in ability.effects)
            {
                if (effect.affectUser)
                    unitHandler.UnitSufferEffect(attacker, effect);
                else
                    unitHandler.UnitSufferEffect(target, effect);
            }

            if (physicalDamage > 0 && !target.isDead)
            {
                unitHandler.UnitSufferDamage(target, physicalDamage);
                DisplayDamageText(physicalDamage, target, attacker.AttackDamageType);
            }
            if (abilityDamage > 0 && !target.isDead)
            {
                unitHandler.UnitSufferAbility(target, ability, abilityDamage);
                DisplayDamageText(abilityDamage, target, ability.damageType);
            }
        }

        StartCoroutine(DelayedTurnEnd());
    }

    public void UnitDeath(Entity target)
    {
        turnHandler.UnitDeath(target);

        //Find dropped items
        totalDroppedExp += Random.Range(target.CharacterData.minExpDrop, target.CharacterData.maxExpDrop);
        if (target.TeamID == 1)
        {
            enemyTeam.Remove(target.GetComponent<CombatAIController>());
        }

        if (enemyTeam.Count <= 0)
        {
            EndCombat();
        }
    }

    public void DisplayDamageText(int damage, Entity target, DamageTypes damageType)
    {
        string dmgText = damage.ToString();
        if (damage <= 0) { dmgText = "MISS"; }
        GameObject dmgDisp = Instantiate(damageTextPrefab, damageTextParent);

        var screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        dmgDisp.GetComponent<RectTransform>().transform.position = new Vector2(screenPos.x, screenPos.y);

        dmgDisp.GetComponent<DamageText>().Setup(dmgText, damageType);
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
    
    private IEnumerator StartCombatDisplay()
    {
        startCombatAnim.gameObject.SetActive(true);
        startCombatAnim.Play("Start Combat");

        if (startCombatAnim != null)
        {
            startCombatAnim.Play("StartCombat");
            yield return new WaitUntil(() => startCombatAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        }

        startCombatAnim.gameObject.SetActive(false);
        CombatEvents.current.StartCombatSetup();
    }

    #endregion
}