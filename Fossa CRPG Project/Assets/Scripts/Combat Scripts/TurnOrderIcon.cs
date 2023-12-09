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
        portrait.sprite = unit.CharacterData.portrait;
        nameDisp.text = Char.CharacterData.Name;
    }
}
