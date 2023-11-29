using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderIcon : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Slider GrayscaleEffect;
    private Entity Char;

    private void Start()
    {
        CombatEvents.current.onNewTurn += NewTurn;
    }

    public void Setup(Entity unit)
    { 
        Char = unit;
        portrait.sprite = unit.CharacterData.portrait;

        var percentageHealthMissing = (Char.activeStatsDir["MaxHP"].baseStatValue - Char.activeStatsDir["MaxHP"].statValue) / Char.activeStatsDir["MaxHP"].baseStatValue;
        GrayscaleEffect.value = percentageHealthMissing;
    }

    private void NewTurn(Entity entity)
    {
        var percentageHealthMissing = (Char.activeStatsDir["MaxHP"].baseStatValue - Char.activeStatsDir["MaxHP"].statValue) / Char.activeStatsDir["MaxHP"].baseStatValue;
        GrayscaleEffect.value = percentageHealthMissing;
    }
}
