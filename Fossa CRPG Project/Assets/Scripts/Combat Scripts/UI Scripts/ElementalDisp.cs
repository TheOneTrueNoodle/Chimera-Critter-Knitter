using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combat;

public class ElementalDisp : MonoBehaviour
{
    public TMP_Text ElementText;
    public Image elementSymbolDisplay;
    [SerializeField] private List<Sprite> damageTypeSymbols;
    public void Setup(DamageTypes damageType)
    {
        switch (damageType)
        {
            case DamageTypes.Smashing:
                ElementText.text = "Smashing";
                elementSymbolDisplay.sprite = damageTypeSymbols[0];
                break;
            case DamageTypes.Slashing:
                ElementText.text = "Slashing";
                elementSymbolDisplay.sprite = damageTypeSymbols[1];
                break;
            case DamageTypes.Stabbing:
                ElementText.text = "Stabbing";
                elementSymbolDisplay.sprite = damageTypeSymbols[2];
                break;
            case DamageTypes.Fire:
                ElementText.text = "Fire";
                elementSymbolDisplay.sprite = damageTypeSymbols[3];
                break;
            case DamageTypes.Water:
                ElementText.text = "Water";
                elementSymbolDisplay.sprite = damageTypeSymbols[4];
                break;
            case DamageTypes.Earth:
                ElementText.text = "Earth";
                elementSymbolDisplay.sprite = damageTypeSymbols[5];
                break;
            case DamageTypes.Wind:
                ElementText.text = "Wind";
                elementSymbolDisplay.sprite = damageTypeSymbols[6];
                break;
            case DamageTypes.Lightning:
                ElementText.text = "Lightning";
                elementSymbolDisplay.sprite = damageTypeSymbols[7];
                break;
            case DamageTypes.Holy:
                ElementText.text = "Holy";
                elementSymbolDisplay.sprite = damageTypeSymbols[8];
                break;
            case DamageTypes.Vile:
                ElementText.text = "Vile";
                elementSymbolDisplay.sprite = damageTypeSymbols[9];
                break;
            case DamageTypes.Positive:
                ElementText.text = "Positive";
                elementSymbolDisplay.sprite = damageTypeSymbols[10];
                break;
            case DamageTypes.Negative:
                ElementText.text = "Negative";
                elementSymbolDisplay.sprite = damageTypeSymbols[11];
                break;
            case DamageTypes.Cosmic:
                ElementText.text = "Cosmic";
                elementSymbolDisplay.sprite = damageTypeSymbols[12];
                break;
            case DamageTypes.Healing:
                ElementText.text = "Healing";
                elementSymbolDisplay.sprite = damageTypeSymbols[13];
                break;
        }
    }
}
