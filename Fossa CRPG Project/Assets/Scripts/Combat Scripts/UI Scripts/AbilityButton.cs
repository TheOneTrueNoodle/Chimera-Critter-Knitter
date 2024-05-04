using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text textShadow;

    public int abilityID;
    public AbilityData ability;
    private HoverTip hoverTip;

    [SerializeField] private FMODUnity.EventReference hoverSFX;
    private void Start()
    {
        hoverTip = GetComponent<HoverTip>();
    }

    public void SetupButton(AbilityData abil, int ID)
    {
        ability = abil;
        abilityID = ID;

        hoverTip = GetComponent<HoverTip>();

        buttonText.text = abil.Name;
        textShadow.text = abil.Name;
        hoverTip.tipToShow = abil.Desc;
    }
    public void Clicked()
    {
        CombatUI combatUI = GetComponentInParent<CombatUI>();

        combatUI.CallAbility(abilityID);
        combatUI.GetComponent<HoverTipManager>().HideTip();
    }
}
