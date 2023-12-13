using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    public Slider xpBar;
    public Animator anim;
    public TMP_Text currentXP;
    public TMP_Text requiredXP;

    public Entity unit;

    public float speed = 3f;

    private void Start()
    {
        CombatEvents.current.onNewTurn += NewTurn;
    }
    public void UpdateInfo()
    {
        currentXP.text = unit.exp.ToString();
        requiredXP.text = unit.requiredExp.ToString();
    }
    public void UpdateBar()
    {
        xpBar.maxValue = unit.requiredExp;
        xpBar.value = unit.exp;
    }
    public void LevelUpEffect()
    {
        anim.Play("Level Up");
    }

    private void NewTurn(Entity newUnit)
    {
        if (newUnit.TeamID == 0)
        {
            unit = newUnit;
            UpdateInfo();
            UpdateBar();
        }
    }

    public IEnumerator AnimateXP(int totalXP, Entity newUnit)
    {
        if(newUnit != null) { unit = newUnit; }

        while (totalXP > 0)
        {
            xpBar.maxValue = unit.requiredExp;
            xpBar.value = unit.exp;
            int startValue = unit.exp;
            int targetValue = unit.exp + totalXP;
            bool levelUp = false;

            if (targetValue >= unit.requiredExp)
            {
                targetValue = unit.requiredExp;
                totalXP -= (unit.requiredExp - unit.exp);
                levelUp = true;
            }
            else { totalXP = 0; }

            float lerpTimer = 0;
            while (lerpTimer < 1)
            {
                lerpTimer += Time.deltaTime;
                float newValue = Mathf.Lerp(startValue, targetValue, lerpTimer);

                xpBar.value = newValue;
                currentXP.text = ((int)newValue).ToString();

                unit.exp = (int)xpBar.value;
                UpdateInfo();
                yield return null;
            }

            if (levelUp && unit.level < unit.CharacterData.levelConfig.MaxLevel)
            {
                unit.exp = 0;
                LevelUpEffect();
                unit.LevelUp();
            }
        }

    }
}
