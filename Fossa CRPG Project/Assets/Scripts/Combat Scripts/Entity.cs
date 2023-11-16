using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Combat;

public class Entity : MonoBehaviour
{
    private TileFunctions tileFunctions = new TileFunctions();

    public OverlayTile activeTile;
    public UnitData CharacterData;
    public bool myTurn = false;
    [HideInInspector] public bool isDead = false;
    public int TeamID; //0 = player ID, 1 = Enemies, 2 = Allies

    //[HideInInspector] public List<EntitySubTile> subTileSpaces;

    private Slider healthbar;

    [HideInInspector] public bool isDefending;

    //ACTIVE STATS
    public Dictionary<string, Stat> activeStatsDir;
    //ACTIVE ABILITIES
    public List<AbilityData> activeAbilities;
    //RESISTANCES AND WEAKNESSES
    public List<DamageTypes> Resistances = new List<DamageTypes>();
    public List<DamageTypes> Weaknesses = new List<DamageTypes>();
    //DAMAGE TYPE
    [HideInInspector] public DamageTypes AttackDamageType;
    [HideInInspector] public int WeaponRange;

    [Header("Level Information")]
    public int level = 1;
    public int exp;
    private int requiredExp;

    private void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onEndCombat += EndCombat;
        }
        healthbar = GetComponentInChildren<Slider>();
    }

    public void UpdateStats()
    {
        CharacterData.SetDictionaryStats(level);
    }

    #region Combat Functions
    public virtual void StartTurn()
    {
        isDefending = false;
        foreach (var item in activeStatsDir.Keys)
        {
            if (activeStatsDir[item].isModified)
            {
                activeStatsDir[item].UpdateStatMods(this);
                updateHealthBar();
                if (activeStatsDir[item].isModified)
                {
                    Debug.Log(item + " isModified = " + activeStatsDir[item].isModified);
                    Debug.Log("Duration " + activeStatsDir[item].statMods[0].duration);
                }
            }
        }

        updateHealthBar();
    }
    #endregion
    #region initialization
    public void Initialize()
    {
        if(level < 1) { level = 1; }
        CharacterData.SetDictionaryStats(level);
        if (CharacterData.Weapon != null)
        {
            AttackDamageType = CharacterData.Weapon.damageType;
            WeaponRange = CharacterData.Weapon.attackRange;
        }
        else
        {
            AttackDamageType = DamageTypes.Smashing;
            WeaponRange = 1;
        }
        if (activeStatsDir == null)
        {
            activeStatsDir = new Dictionary<string, Stat>();
            foreach (KeyValuePair<string, Stat> item in CharacterData.statsDir)
            {
                activeStatsDir.Add(item.Value.name, new Stat(item.Value.name, item.Value.baseStatValue));
            }
        }
        CharacterData.SetEquipment();
        activeAbilities = CharacterData.SetAbilities(null);
        Resistances = CharacterData.SetResistances();
        Weaknesses = CharacterData.SetWeaknesses();

        List<EquipmentStatChanges> equipmentStatChanges = CharacterData.SetEquipmentStatChanges();
        List<ScriptableEffect> equipmentEffects = CharacterData.SetStartingEffects();

        CombatEvents.current.UnitStartingEffects(this, equipmentStatChanges, equipmentEffects);
        var focusedTileHit = tileFunctions.GetSingleFocusedOnTile(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10f, gameObject.transform.position.z), true);
        CombatEvents.current.TilePositionEntity(this, focusedTileHit);
        updateHealthBar();
    }

    private void EndCombat()
    {
        healthbar.gameObject.SetActive(false);
        activeTile = null;
        UpdateStats();
    }
    #endregion

    #region EXP AND LEVELING
    public void IncreaseEXP(int xp)
    {
        if(exp >= requiredExp && level < CharacterData.levelConfig.MaxLevel)
        {
            exp += xp;
            while (exp >= requiredExp)
            {
                exp -= requiredExp;
                LevelUp();
            }
        }
    }
    public void LevelUp()
    {
        level++;
        CalculateRequiredEXP();
    }
    public void CalculateRequiredEXP()
    {
        requiredExp = CharacterData.levelConfig.GetRequiredExp(level);
    }
    #endregion
    #region extra functions
    public void updateHealthBar()
    {
        if (healthbar != null)
        {
            healthbar.maxValue = activeStatsDir["MaxHP"].baseStatValue;
            healthbar.value = activeStatsDir["MaxHP"].statValue;
            if (activeStatsDir["MaxHP"].statValue >= healthbar.maxValue || activeStatsDir["MaxHP"].statValue <= 0) { healthbar.gameObject.SetActive(false); }
            else { healthbar.gameObject.SetActive(true); }
        }
    }
    #endregion
}
