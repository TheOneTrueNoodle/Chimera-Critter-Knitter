using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMenuManager : MonoBehaviour
{
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
}
