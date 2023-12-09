using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoUI : MonoBehaviour
{
    [Header("Unit Information Display")]
    public Image portrait;
    public TMP_Text Name;
    public Slider HPBar;
    public TMP_Text HPText;
    public Slider SPBar;
    public TMP_Text SPText; 

    public void UpdateUI(Entity Char)
    {
        if (Char != null)
        {
            portrait.sprite = Char.CharacterData.portrait;
            Name.text = Char.CharacterData.Name;

            HPBar.maxValue = (int)Char.activeStatsDir["MaxHP"].baseStatValue;
            HPBar.value = Char.activeStatsDir["MaxHP"].statValue;
            HPText.text = HPBar.value.ToString() + " / " + HPBar.maxValue.ToString();

            if (Char.activeStatsDir["MaxSP"].baseStatValue == 0) { SPBar.gameObject.SetActive(false); }
            else { SPBar.gameObject.SetActive(true); }

            SPBar.maxValue = (int)Char.activeStatsDir["MaxSP"].baseStatValue;
            SPBar.value = Char.activeStatsDir["MaxSP"].statValue;
            SPText.text = SPBar.value.ToString() + " / " + SPBar.maxValue.ToString();
        }
        else
        {
            Debug.LogError("No Character To Display UI");
        }
    }
}
