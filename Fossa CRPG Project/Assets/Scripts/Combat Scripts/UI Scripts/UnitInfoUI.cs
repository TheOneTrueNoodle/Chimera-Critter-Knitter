using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoUI : MonoBehaviour
{
    public bool PlayerInfoUI;
    private bool inCombat = false;

    [Header("Basic Information")]
    public Image portrait;

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

    //Private variables
    public Entity currentChar;
    public bool HPAnimating;
    public float HPDelay;
    public float HPOldValue;

    public bool SPAnimating;
    public float SPDelay;
    public float SPOldValue;

    private void Start()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
        if (PlayerInfoUI) { currentChar = FindObjectOfType<PlayerMovement>().GetComponent<Entity>(); }
    }

    private void Update()
    {
        if (!inCombat && PlayerInfoUI)
        {
            if (currentChar == null) { UpdateUI(FindObjectOfType<PlayerMovement>().GetComponent<Entity>()); }
            
        }
        else if (inCombat && currentChar != null)
        {
            UpdateUI(currentChar);
        }
    }

    public void UpdateUI(Entity Char)
    {
        if (Char != null)
        {
            if (Char.activeStatsDir == null)
            {
                Debug.Log("Getting new stats");
                Char.UpdateStats();
            }

            //Basic Updates
            float percentageHealthMissing = ((Char.activeStatsDir["MaxHP"].baseStatValue - Char.activeStatsDir["MaxHP"].statValue) / Char.activeStatsDir["MaxHP"].baseStatValue) * 100;
            if (percentageHealthMissing > 50 && PlayerPrefs.GetInt("Remove Gore") != 1)
            {
                portrait.sprite = Char.CharacterData.injuredPortrait;
            }
            else
            {
                portrait.sprite = Char.CharacterData.portrait;
            }

            //HP Update
            UpdateHP(Char);

            //SP Update
            UpdateSP(Char);

            //XP Updates
            if (EXPBar != null)
            {
                EXPBar.unit = Char;
                if (!inCombat) { EXPBar.UpdateBar(); }
            }

            UpdateBars();

            currentChar = Char;
        }
        else
        {
            Debug.LogError("No Character To Display UI");
        }
    }

    public void CloseUI()
    {
        currentChar = null;
        gameObject.SetActive(false);
    }

    private void UpdateHP(Entity Char)
    {
        //HP Updates
        if (Char == currentChar)
        {
            if (HPBarDelayed.value != HPBar.value && !HPAnimating)
            {
                HPAnimating = true;
                HPDelay = 0;
                HPOldValue = HPBar.value;
                HPBarDelayed.maxValue = HPBar.maxValue;
            }
        }
        else
        {
            HPBarDelayed.maxValue = Char.activeStatsDir["MaxHP"].baseStatValue;
            HPBarDelayed.value = Char.activeStatsDir["MaxHP"].statValue;
        }
        HPBar.maxValue = (int)Char.activeStatsDir["MaxHP"].baseStatValue;
        HPBar.value = Char.activeStatsDir["MaxHP"].statValue;
        HPText.text = HPBar.value.ToString() + "/" + HPBar.maxValue.ToString();
    }

    private void UpdateSP(Entity Char)
    {
        //SP Updates
        if (SPBar != null && SPBarDelayed != null && SPText != null)
        {
            if (Char.activeStatsDir["MaxSP"].baseStatValue == 0) { SPBar.gameObject.SetActive(false); }
            else { SPBar.gameObject.SetActive(true); }

            if (Char == currentChar)
            {
                if (SPBar.value != Char.activeStatsDir["MaxSP"].statValue && !SPAnimating) 
                {
                    SPAnimating = true;
                    SPDelay = 0;
                    SPOldValue = SPBar.value;
                }
            }
            else
            {
                SPBarDelayed.maxValue = Char.activeStatsDir["MaxSP"].baseStatValue;
                SPBarDelayed.value = Char.activeStatsDir["MaxSP"].statValue;
            }
            SPBar.maxValue = (int)Char.activeStatsDir["MaxSP"].baseStatValue;
            SPBar.value = Char.activeStatsDir["MaxSP"].statValue;
            SPText.text = SPBar.value.ToString() + "/" + SPBar.maxValue.ToString();
        }
    }

    private void UpdateBars()
    {
        if (HPAnimating)
        {
            if (HPDelay > 1f)
            {
                //Lerp the bars together.
                HPBarDelayed.value = Mathf.Lerp(HPOldValue, HPBar.value, (Time.deltaTime + (HPDelay - 1)) * 4f);
                HPDelay += Time.deltaTime;

                if (HPDelay > 2f)
                {
                    HPBarDelayed.value = HPBar.value;
                    HPDelay = 0;
                    HPOldValue = HPBar.value;
                    HPAnimating = false;
                }
            }
            else
            {
                HPDelay += Time.deltaTime;
            }
        }

        if (SPAnimating)
        {
            if (SPDelay > 1f)
            {
                //Lerp the bars together.
                SPBarDelayed.value = Mathf.Lerp(SPOldValue, SPBar.value, (Time.deltaTime + (SPDelay - 1)) * 4f);
                if (Mathf.Abs(SPBarDelayed.value) - Mathf.Abs(SPBar.value) > -1 && Mathf.Abs(SPBarDelayed.value) - Mathf.Abs(SPBar.value) < 1)
                {
                    SPBarDelayed.value = SPBar.value;
                    SPAnimating = false;
                }
            }
            else
            {
                SPDelay += Time.deltaTime;
            }
        }
    }

    private void StartCombat(string combatName)
    {
        inCombat = true;
        if (!PlayerInfoUI)
        {
            currentChar = null;
        }
    }
    private void EndCombat(string combatName)
    {
        inCombat = false;
        currentChar = FindObjectOfType<PlayerMovement>().GetComponent<Entity>();
    }
}
