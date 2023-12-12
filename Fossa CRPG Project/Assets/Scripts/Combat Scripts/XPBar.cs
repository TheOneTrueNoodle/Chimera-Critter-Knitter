using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider xpBar;
    public Animator anim;

    public float targetXP;
    public Entity unit;

    public float speed = 3f;
    private float lerpTimer;

    private void Start()
    {
        CombatEvents.current.onNewTurn += NewTurn;
    }

    private void Update()
    {
        if (xpBar.value != targetXP && unit != null)
        {
            IncreaseEXP();
        }
    }

    public void IncreaseEXP()
    {

    }

    private void UpdateInfo()
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
        }
    }
}
