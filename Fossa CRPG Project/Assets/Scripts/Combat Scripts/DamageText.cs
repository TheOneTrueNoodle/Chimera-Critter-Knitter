using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combat;

public class DamageText : MonoBehaviour
{
    public TMP_Text damageText;
    public Image damageTypeDisplay;
    [SerializeField] private List<Sprite> damageTypeSymbols;
    [SerializeField] private List<Color> damageTypeColors;

    private void Update()
    {
        if (Camera.main is { }) transform.LookAt(-Camera.main.transform.position, Vector3.up);
    }

    public void Setup(string Damage, DamageTypes damageType)
    {
        damageText.text = Damage;

        switch (damageType)
        {
            case DamageTypes.Smashing:
                damageText.faceColor = damageTypeColors[0];
                damageTypeDisplay.sprite = damageTypeSymbols[0];
                break;
            case DamageTypes.Slashing:
                damageText.faceColor = damageTypeColors[1];
                damageTypeDisplay.sprite = damageTypeSymbols[1];
                break;
            case DamageTypes.Stabbing:
                damageText.faceColor = damageTypeColors[2];
                damageTypeDisplay.sprite = damageTypeSymbols[2];
                break;
            case DamageTypes.Fire:
                damageText.faceColor = damageTypeColors[3];
                damageTypeDisplay.sprite = damageTypeSymbols[3];
                break;
            case DamageTypes.Water:
                damageText.faceColor = damageTypeColors[4];
                damageTypeDisplay.sprite = damageTypeSymbols[4];
                break;
            case DamageTypes.Earth:
                damageText.faceColor = damageTypeColors[5];
                damageTypeDisplay.sprite = damageTypeSymbols[5];
                break;
            case DamageTypes.Wind:
                damageText.faceColor = damageTypeColors[6];
                damageTypeDisplay.sprite = damageTypeSymbols[6];
                break;
            case DamageTypes.Lightning:
                damageText.faceColor = damageTypeColors[7];
                damageTypeDisplay.sprite = damageTypeSymbols[7];
                break;
            case DamageTypes.Holy:
                damageText.faceColor = damageTypeColors[8];
                damageTypeDisplay.sprite = damageTypeSymbols[8];
                break;
            case DamageTypes.Vile:
                damageText.faceColor = damageTypeColors[9];
                damageTypeDisplay.sprite = damageTypeSymbols[9];
                break;
            case DamageTypes.Positive:
                damageText.faceColor = damageTypeColors[10];
                damageTypeDisplay.sprite = damageTypeSymbols[10];
                break;
            case DamageTypes.Negative:
                damageText.faceColor = damageTypeColors[11];
                damageTypeDisplay.sprite = damageTypeSymbols[11];
                break;
            case DamageTypes.Cosmic:
                damageText.faceColor = damageTypeColors[12];
                damageTypeDisplay.sprite = damageTypeSymbols[12];
                break;
            case DamageTypes.Healing:
                damageText.faceColor = damageTypeColors[13];
                damageTypeDisplay.sprite = damageTypeSymbols[13];
                break;
        }
    }
}
