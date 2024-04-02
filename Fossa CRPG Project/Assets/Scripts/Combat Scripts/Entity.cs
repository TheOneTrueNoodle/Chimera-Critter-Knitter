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

    public Animator anim;

    [HideInInspector] public bool isDefending;

    [Header("Active Stats")]
    public Dictionary<string, Stat> activeStatsDir;
    [Header("Active Abilities")]
    public List<AbilityData> activeAbilities;

    [Header("Equipment")]
    public ScriptableWeapon heldWeapon;

    [Header("Resistances & Weaknesses")]
    public List<DamageTypes> Resistances = new List<DamageTypes>();
    public List<DamageTypes> Weaknesses = new List<DamageTypes>();
    
    //Damage Types
    [HideInInspector] public DamageTypes AttackDamageType;
    [HideInInspector] public int WeaponRange;

    [Header("Level Information")]
    public int level = 1;
    public int exp;
    public int requiredExp;

    [Header("Visuals")]
    public GameObject GFX;
    public Transform UITarget;
    public ParticleSystem bloodSplatter;

    [HideInInspector] public FootstepInstance footstepInstance;

    private void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onEndCombat += EndCombat;
        }
        if (anim == null && GetComponentInChildren<Animator>() != null) { anim = GetComponentInChildren<Animator>(); }
        footstepInstance = GetComponent<FootstepInstance>();
        CalculateRequiredEXP();
    }

    public void UpdateStats()
    {
        CharacterData.SetDictionaryStats(level);
        if (activeStatsDir == null)
        {
            Debug.Log("It thinks active stats is null");
            activeStatsDir = new Dictionary<string, Stat>();
            foreach (KeyValuePair<string, Stat> item in CharacterData.statsDir)
            {
                activeStatsDir.Add(item.Value.name, new Stat(item.Value.name, item.Value.baseStatValue));
            }
        }
        else
        {
            foreach (KeyValuePair<string, Stat> item in CharacterData.statsDir)
            {
                if (item.Value.name == "MaxHP" || item.Value.name == "MaxSP")
                {
                    var difference = activeStatsDir[item.Value.name].baseStatValue - activeStatsDir[item.Value.name].statValue;
                    float currentValue = activeStatsDir[item.Value.name].statValue;

                    activeStatsDir[item.Value.name] = item.Value;
                    if(difference != 0) { activeStatsDir[item.Value.name].statValue = currentValue; }
                }
                else
                {
                    activeStatsDir[item.Value.name] = item.Value;
                }
            }
        }
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
                if (activeStatsDir[item].isModified)
                {
                    Debug.Log(item + " isModified = " + activeStatsDir[item].isModified);
                    Debug.Log("Duration " + activeStatsDir[item].statMods[0].duration);
                }
            }
        }
    }

    public void BloodSplash() 
    {
        if (bloodSplatter != null)
        {
            bloodSplatter.Play();
        }
        else { Debug.LogError("No assigned blood splatter"); }
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
            AttackDamageType = CharacterData.defaultAttack;
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
        else
        {
            UpdateStats();
        }

        CharacterData.SetEquipment();
        activeAbilities = CharacterData.SetAbilities(null);
        Resistances = CharacterData.SetResistances();
        Weaknesses = CharacterData.SetWeaknesses();

        //EQUIPMENT / HELD ITEMS
        List<EquipmentStatChanges> equipmentStatChanges = SetEquipmentStatChanges();
        List<ScriptableEffect> equipmentEffects = SetStartingEffects();

        CombatEvents.current.UnitStartingEffects(this, equipmentStatChanges, equipmentEffects);
        var focusedTileHit = tileFunctions.GetSingleFocusedOnTile(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10f, gameObject.transform.position.z), true);
        CombatEvents.current.TilePositionEntity(this, focusedTileHit);

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.GetComponent<Collider>().enabled = false;

        if (anim != null)
        {
            anim.SetBool("InCombat", true);
        }
    }
    public List<EquipmentStatChanges> SetEquipmentStatChanges()
    {
        List<EquipmentStatChanges> returnStatChanges = new List<EquipmentStatChanges>();
        if (heldWeapon != null)
        {
            foreach (EquipmentStatChanges statChange in heldWeapon.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }
        /*
        if (Armour != null)
        {
            foreach (EquipmentStatChanges statChange in Armour.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }
        if (Accessory1 != null)
        {
            foreach (EquipmentStatChanges statChange in Accessory1.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }
        if (Accessory2 != null)
        {
            foreach (EquipmentStatChanges statChange in Accessory2.StatChanges)
            {
                returnStatChanges.Add(statChange);
            }
        }*/

        return returnStatChanges;
    }
    public List<ScriptableEffect> SetStartingEffects()
    {
        List<ScriptableEffect> returnEffects = new List<ScriptableEffect>();
        if (heldWeapon != null)
        {
            foreach (ScriptableEffect effect in heldWeapon.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }
        /*
        if (Armour != null)
        {
            foreach (ScriptableEffect effect in Armour.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }
        if (Accessory1 != null)
        {
            foreach (ScriptableEffect effect in Accessory1.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }
        if (Accessory2 != null)
        {
            foreach (ScriptableEffect effect in Accessory2.AdditionalEffects)
            {
                returnEffects.Add(effect);
            }
        }*/

        return returnEffects;
    }
    public void Die()
    {
        if (anim != null)
        {
            anim.SetBool("Dead", true);
        }

        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = false;
        }

        if (TryGetComponent(out DestroyOffScreen destroyOffScreen))
        {
            destroyOffScreen.enabled = true;
        }
    }

    private void EndCombat(string combatName)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if(anim != null) { anim.SetBool("InCombat", false); }
        //gameObject.GetComponent<Collider>().enabled = true;
    }
    #endregion

    #region EXP AND LEVELING
    public void IncreaseEXP(XPBar bar, int xp)
    {
        if (bar != null)
        {
            StartCoroutine(bar.AnimateXP(xp, this));
        }
        else
        {
            exp += xp;
            while (exp >= requiredExp && level < CharacterData.levelConfig.MaxLevel)
            {
                exp -= requiredExp;
                Debug.Log("Leveling up through entity code");
                LevelUp();
            }
        }
    }
    public void LevelUp()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.levelUpSFX, transform.position);
        level++;

        UpdateStats();
        CalculateRequiredEXP();
    }
    public void CalculateRequiredEXP()
    {
        requiredExp = CharacterData.levelConfig.GetRequiredExp(level);
    }
    #endregion

    private void OnDestroy()
    {
        CombatEvents.current.onEndCombat -= EndCombat;
    }
}
