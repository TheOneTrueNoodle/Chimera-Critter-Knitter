using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combat;

public class AbilityDisp : MonoBehaviour
{
    public TMP_Text AbilityText;
    public HoverTip hoverTip;
    private AbilityData ability;

    public void Setup(AbilityData abil)
    {
        ability = abil;

        AbilityText.text = abil.Name;
        hoverTip.tipToShow = abil.Desc;
    }
}
