using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoUI : MonoBehaviour
{
    [Header("Basic Information")]
    public Image portrait;
    public TMP_Text Name;
    public TMP_Text Level;

    [Header("HP Variables")]
    public Slider HPBar;
    public Slider HPBarDelayed;
    public TMP_Text HPText;

    [Header("SP Variables")]
    public Slider SPBar;
    public Slider SPBarDelayed;
    public TMP_Text SPText;

    [Header("XP Variables")]
    public XPBar EXPBar;
    public TMP_Text XPCurrentText;
    public TMP_Text XPRequiredText;

    //Private variables
    public Entity currentChar;

    public void UpdateUI(Entity Char)
    {
        if (Char != null)
        {
            if (Char.activeStatsDir == null)
            {
                Char.UpdateStats();
            }

            //Basic Updates
            portrait.sprite = Char.CharacterData.portrait;
            Name.text = Char.CharacterData.Name;
            Level.text = "LV " + Char.level.ToString();

            //HP Updates
            if (Char == currentChar)
            {
                if (HPBar.value != Char.activeStatsDir["MaxHP"].statValue) { StartCoroutine(SliderCatchup(HPBarDelayed, Char.activeStatsDir["MaxHP"].statValue, HPBar.value, (int)Char.activeStatsDir["MaxHP"].baseStatValue)); }
            }
            else 
            {
                HPBarDelayed.value = Char.activeStatsDir["MaxHP"].statValue; 
            }
            HPBar.maxValue = (int)Char.activeStatsDir["MaxHP"].baseStatValue;
            HPBar.value = Char.activeStatsDir["MaxHP"].statValue;
            HPText.text = HPBar.value.ToString() + " / " + HPBar.maxValue.ToString();

            //SP Updates
            if (Char.activeStatsDir["MaxSP"].baseStatValue == 0) { SPBar.gameObject.SetActive(false); }
            else { SPBar.gameObject.SetActive(true); }

            if (Char == currentChar)
            {
                if (SPBar.value != Char.activeStatsDir["MaxSP"].statValue) { StartCoroutine(SliderCatchup(SPBarDelayed, Char.activeStatsDir["MaxSP"].statValue, SPBar.value, Char.activeStatsDir["MaxSP"].baseStatValue)); }
            }
            else { SPBarDelayed.value = Char.activeStatsDir["MaxSP"].statValue; }
            SPBar.maxValue = (int)Char.activeStatsDir["MaxSP"].baseStatValue;
            SPBar.value = Char.activeStatsDir["MaxSP"].statValue;
            SPText.text = SPBar.value.ToString() + " / " + SPBar.maxValue.ToString();

            //XP Updates
            if (EXPBar != null)
            {
                XPCurrentText.text = Char.exp.ToString();
                XPRequiredText.text = Char.requiredExp.ToString();
            }

            currentChar = Char;
        }
        else
        {
            Debug.LogError("No Character To Display UI");
        }
    }

    private IEnumerator<WaitForSecondsRealtime> SliderCatchup(Slider Slider, float targetValue, float previousValue, float maxValue)
    {
        Debug.LogWarning("STARTED ANIMATED BAR COROUTINE");

        Slider.maxValue = maxValue;
        Slider.value = previousValue;

        yield return new WaitForSecondsRealtime(1f);

        float lerpTimer = 0;
        while(lerpTimer < 1)
        {
            lerpTimer += Time.deltaTime;
            Slider.value = Mathf.Lerp(Slider.value, targetValue, lerpTimer * 4);
            if(Mathf.Abs(Slider.value) - Mathf.Abs(targetValue) > -1 && Mathf.Abs(Slider.value) - Mathf.Abs(targetValue) < 1)
            {
                Slider.value = targetValue;
            }
            yield return null;
        }
    }
}
