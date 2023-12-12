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
    private void UpdateBar()
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
}
