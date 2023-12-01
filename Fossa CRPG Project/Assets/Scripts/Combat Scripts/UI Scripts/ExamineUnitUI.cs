using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combat;

public class ExamineUnitUI : MonoBehaviour
{
    [HideInInspector] public bool Displayed;

    [Header("Major Displays")]
    [SerializeField] private Image Portrait;
    [SerializeField] private TMP_Text UnitName;
    [SerializeField] private TMP_Text LevelDisp;
    [SerializeField] private TMP_Text HPDisp;
    [HideInInspector] public Entity Unit;

    [Header("Section UI")]
    [SerializeField] private Transform AbilitiesContent;
    [SerializeField] private Transform ResistancesContent;
    [SerializeField] private Transform WeaknessesContent;

    [Header("Prefabs")]
    [SerializeField] private ElementalDisp ElementDisp;
    [SerializeField] private AbilityDisp AbilityDisp;

    private List<GameObject> ActiveInstantiatedObjects = new List<GameObject>();

    public void AssignEntity(OverlayTile tile)
    {
        Unit = tile.activeCharacter;

        //Basic Info
        Portrait.sprite = Unit.CharacterData.portrait;
        UnitName.text = Unit.CharacterData.Name;
        LevelDisp.text = "Level: " + Unit.level;
        HPDisp.text = "HP: " + (int)Unit.activeStatsDir["MaxHP"].baseStatValue + " / " + (int)Unit.activeStatsDir["MaxHP"].statValue;

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
        foreach (DamageTypes damageType in Unit.Resistances)
        {
            ElementalDisp newResistance = Instantiate(ElementDisp, ResistancesContent);
            newResistance.Setup(damageType);
            ActiveInstantiatedObjects.Add(newResistance.gameObject);
        }
        foreach (DamageTypes damageType in Unit.Weaknesses)
        {
            ElementalDisp newWeakness = Instantiate(ElementDisp, WeaknessesContent);
            newWeakness.Setup(damageType);
            ActiveInstantiatedObjects.Add(newWeakness.gameObject);
        }

        //Setup Abilities
        foreach (AbilityData ability in Unit.activeAbilities)
        {
            AbilityDisp newAbil = Instantiate(AbilityDisp, AbilitiesContent);
            newAbil.Setup(ability);
            ActiveInstantiatedObjects.Add(newAbil.gameObject);
        }
    }
}
