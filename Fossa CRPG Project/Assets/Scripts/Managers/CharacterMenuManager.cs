using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combat;

public class CharacterMenuManager : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private Image portrait;
    [SerializeField] private Character oscarChar;
    private Entity oscarData;

    [Header("Level and XP")]
    [SerializeField] private TMP_Text levelDisp;
    [SerializeField] private TMP_Text xpDisp;
    [SerializeField] private Slider xpBar;

    [Header("HP and SP")]
    [SerializeField] private TMP_Text hpDisp;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TMP_Text spDisp;
    [SerializeField] private Slider spBar;

    [Header("Section UI")]
    [SerializeField] private Transform resistancesContent;
    [SerializeField] private Transform weaknessesContent;

    [Header("Prefabs")]
    [SerializeField] private ElementalDisp ElementDisp;

    private List<GameObject> ActiveInstantiatedObjects = new List<GameObject>();

    public void UpdateDisplay()
    {
        if(oscarData == null) { oscarData = FindObjectOfType<PlayerMovement>().GetComponent<Entity>(); }

        if(oscarData.activeStatsDir == null) { AssignOscarData(); }

        //Basic Info
        float percentageHealthMissing = ((oscarData.activeStatsDir["MaxHP"].baseStatValue - oscarData.activeStatsDir["MaxHP"].statValue) / oscarData.activeStatsDir["MaxHP"].baseStatValue) * 100;
        if (percentageHealthMissing > 50)
        {
            portrait.sprite = oscarData.CharacterData.injuredPortrait;
        }
        else
        {
            portrait.sprite = oscarChar.defaultPortrait;
        }

        levelDisp.text = "LV: " + oscarData.level;
        //HP
        hpDisp.text = (int)oscarData.activeStatsDir["MaxHP"].statValue + "\n---\n" + (int)oscarData.activeStatsDir["MaxHP"].baseStatValue;
        hpBar.maxValue = oscarData.activeStatsDir["MaxHP"].baseStatValue;
        hpBar.value = oscarData.activeStatsDir["MaxHP"].statValue;

        //SP
        spDisp.text = (int)oscarData.activeStatsDir["MaxSP"].statValue + "\n---\n" + (int)oscarData.activeStatsDir["MaxSP"].baseStatValue;
        spBar.maxValue = oscarData.activeStatsDir["MaxSP"].baseStatValue;
        spBar.value = oscarData.activeStatsDir["MaxSP"].statValue;

        //XP
        xpDisp.text = (int)oscarData.exp + " / " + (int)oscarData.requiredExp;
        xpBar.maxValue = oscarData.requiredExp;
        xpBar.value = oscarData.exp;

        //Clear older UI Elements
        if (ActiveInstantiatedObjects.Count > 0)
        {
            foreach (GameObject obj in ActiveInstantiatedObjects)
            {
                Destroy(obj);
            }
            ActiveInstantiatedObjects.Clear();
        }

        //Setup weaknesses and resistances
        if (oscarData.Resistances.Count != 0)
        {
            foreach (DamageTypes damageType in oscarData.Resistances)
            {
                ElementalDisp newResistance = Instantiate(ElementDisp, resistancesContent);
                newResistance.Setup(damageType);
                ActiveInstantiatedObjects.Add(newResistance.gameObject);
            }
        }
        if (oscarData.Weaknesses.Count != 0)
        {
            foreach (DamageTypes damageType in oscarData.Weaknesses)
            {
                ElementalDisp newWeakness = Instantiate(ElementDisp, weaknessesContent);
                newWeakness.Setup(damageType);
                ActiveInstantiatedObjects.Add(newWeakness.gameObject);
            }
        }
    }

    private void AssignOscarData()
    {
        if (oscarData.level < 1) { oscarData.level = 1; }
        oscarData.CharacterData.SetDictionaryStats(oscarData.level);
        if (oscarData.CharacterData.Weapon != null)
        {
            oscarData.AttackDamageType = oscarData.CharacterData.Weapon.damageType;
            oscarData.WeaponRange = oscarData.CharacterData.Weapon.attackRange;
        }
        else
        {
            oscarData.AttackDamageType = oscarData.CharacterData.defaultAttack;
            oscarData.WeaponRange = 1;
        }

        if (oscarData.activeStatsDir == null)
        {
            oscarData.activeStatsDir = new Dictionary<string, Stat>();
            foreach (KeyValuePair<string, Stat> item in oscarData.CharacterData.statsDir)
            {
                oscarData.activeStatsDir.Add(item.Value.name, new Stat(item.Value.name, item.Value.baseStatValue));
            }
        }

        oscarData.CharacterData.SetEquipment();
        oscarData.activeAbilities = oscarData.CharacterData.SetAbilities(null);
        oscarData.Resistances = oscarData.CharacterData.SetResistances();
        oscarData.Weaknesses = oscarData.CharacterData.SetWeaknesses();

        List<EquipmentStatChanges> equipmentStatChanges = oscarData.CharacterData.SetEquipmentStatChanges();
        List<ScriptableEffect> equipmentEffects = oscarData.CharacterData.SetStartingEffects();

        CombatEvents.current.UnitStartingEffects(oscarData, equipmentStatChanges, equipmentEffects);
    }
}
