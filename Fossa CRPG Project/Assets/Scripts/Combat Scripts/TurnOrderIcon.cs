using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnOrderIcon : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private TMP_Text nameDisp;
    private Entity Char;

    public void Setup(Entity unit)
    {
        Char = unit;
        float percentageHealthMissing = ((Char.activeStatsDir["MaxHP"].baseStatValue - Char.activeStatsDir["MaxHP"].statValue) / Char.activeStatsDir["MaxHP"].baseStatValue) * 100;
        if (percentageHealthMissing > 50)
        {
            portrait.sprite = Char.CharacterData.injuredPortrait;
        }
        else
        {
            portrait.sprite = Char.CharacterData.portrait;
        }
        nameDisp.text = Char.CharacterData.Name;
    }
}
