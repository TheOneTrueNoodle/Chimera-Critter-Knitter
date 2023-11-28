using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    private TMP_Text buttonText;

    public int abilityID;
    public AbilityData ability;
    private HoverTip hoverTip;

    private void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        hoverTip = GetComponent<HoverTip>();
    }

    public void SetupButton(AbilityData abil, int ID)
    {
        ability = abil;
        abilityID = ID;

        buttonText = GetComponentInChildren<TMP_Text>();
        hoverTip = GetComponent<HoverTip>();

        buttonText.text = abil.Name;
        hoverTip.tipToShow = abil.Desc;
    }
    public void Clicked()
    {
        CombatUI combatUI = GetComponentInParent<CombatUI>();

        combatUI.CallAbility(abilityID);
        combatUI.GetComponent<HoverTipManager>().HideTip();
    }
}
