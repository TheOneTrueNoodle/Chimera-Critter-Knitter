using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleTips : MonoBehaviour
{
    [SerializeField] private TMP_Text oscarHitText;
    [SerializeField] private TMP_Text enemyHitText;

    private int oscarTargetPercent;
    private int enemyTargetPercent;

    private bool displayAttackHitChance;
    public float attackHitChanceTimer;


    public void AttackHitChanceDisplay(Entity attacker, Entity defender)
    {
        if (displayAttackHitChance)
        {
            AnimateAttackHitChance();
            return;
        }
        //I want it to tick up from 0% over the course of like half a second
        displayAttackHitChance = true;

        oscarTargetPercent = CalculateHitChance(attacker, defender);
        enemyTargetPercent = CalculateHitChance(defender, attacker);

        attackHitChanceTimer = 0;

        oscarHitText.gameObject.SetActive(true);
        enemyHitText.gameObject.SetActive(true);
    }

    private int CalculateHitChance(Entity attacker, Entity defender)
    {
        //FORMULA Attacker Accuracy / Attacker Accuracy - (1 - Defender Evasion / Attacker Accuracy)
        float accuracy = attacker.activeStatsDir["Accuracy"].statValue;
        float evasion = defender.activeStatsDir["Dodge"].statValue;

        float hitChance = 1 - (evasion / (3 * accuracy));
        if(hitChance < 0.05f) { hitChance = 0.05f; }
        hitChance *= 100;

        return (int)hitChance;
    }

    private void AnimateAttackHitChance()
    {
        if (attackHitChanceTimer <= 1f)
        {
            attackHitChanceTimer += Time.deltaTime * 2;
            oscarHitText.text = ((int)Mathf.Lerp(0, oscarTargetPercent, attackHitChanceTimer)).ToString() + "%";
            enemyHitText.text = ((int)Mathf.Lerp(0, enemyTargetPercent, attackHitChanceTimer)).ToString() + "%";
        }
    }

    public void HideAttackHitChanceDisplay()
    {
        displayAttackHitChance = false;
        oscarTargetPercent = 0;
        enemyTargetPercent = 0;

        oscarHitText.gameObject.SetActive(false);
        enemyHitText.gameObject.SetActive(false);
    }
}
