using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Combat;

public class BattleTips : MonoBehaviour
{
    [Header("Oscar")]
    [SerializeField] private GameObject oscarTipsUI;
    [SerializeField] private TMP_Text oscarHitText;
    [SerializeField] private TMP_Text oscarDmgText;

    
    [Header("Enemy")]
    [SerializeField] private GameObject enemyTipsUI;
    [SerializeField] private TMP_Text enemyHitText;
    [SerializeField] private TMP_Text enemyDmgText;

    [Header("Damage Symbols")]
    [SerializeField] private List<Sprite> damageSymbols;

    //Oscar private variables
    private int oscarTargetPercent;
    private int oscarTargetDamage;

    //Enemy private variables
    private int enemyTargetPercent;
    private int enemyTargetDamage;

    private bool displayAttackHitChance;
    [HideInInspector] public float attackHitChanceTimer;



    public void AttackHitChanceDisplay(Entity attacker, Entity defender)
    {
        if (displayAttackHitChance)
        {
            AnimateText();
            return;
        }
        //I want it to tick up from 0% over the course of like half a second
        displayAttackHitChance = true;

        //Oscar calculations
        oscarTargetPercent = CalculateHitChance(attacker, defender);
        oscarTargetDamage = CalculateDamage(attacker, defender);

        //Enemy calculations
        enemyTargetPercent = CalculateHitChance(defender, attacker);
        enemyTargetDamage = CalculateDamage(defender, attacker);

        attackHitChanceTimer = 0;

        //Oscar displays
        oscarTipsUI.SetActive(true);

        //Enemy displays
        enemyTipsUI.SetActive(true);
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

    private int CalculateDamage(Entity attacker, Entity defender)
    {
        int rawDamage = (int)attacker.activeStatsDir["Attack"].statValue;
        if (attacker.CharacterData.Weapon != null) { rawDamage += attacker.CharacterData.Weapon.weaponDamage; }

        int takenDamage;
        if ((int)attacker.AttackDamageType < 3)
        {
            takenDamage = (int)(rawDamage * 100 / (100 + defender.activeStatsDir["Defence"].statValue));
        }
        else
        {
            takenDamage = (int)(rawDamage * 100 / (100 + defender.activeStatsDir["MagicDefence"].statValue));
        }

        takenDamage = (int)(takenDamage * (defender.isDefending == true ? 0.5 : 1));
        if (defender.Resistances.Contains(attacker.AttackDamageType)) { takenDamage /= 2; }
        if (defender.Weaknesses.Contains(attacker.AttackDamageType)) { takenDamage *= 2; }

        if (takenDamage < 0) { takenDamage = 0; }

        return takenDamage;
    }

    private void AnimateText()
    {
        if (attackHitChanceTimer <= 1f)
        {
            attackHitChanceTimer += Time.deltaTime * 2;

            //Oscar
            oscarHitText.text = ((int)Mathf.Lerp(0, oscarTargetPercent, attackHitChanceTimer)).ToString() + "%";
            oscarDmgText.text = ((int)Mathf.Lerp(0, oscarTargetDamage, attackHitChanceTimer)).ToString();

            //Enemy
            enemyHitText.text = ((int)Mathf.Lerp(0, enemyTargetPercent, attackHitChanceTimer)).ToString() + "%";
            enemyDmgText.text = ((int)Mathf.Lerp(0, enemyTargetDamage, attackHitChanceTimer)).ToString();
        }
    }

    public void HideAttackHitChanceDisplay()
    {
        displayAttackHitChance = false;

        //Oscar variables
        oscarTargetPercent = 0;
        oscarTargetDamage = 0;
        
        //Enemy variables
        enemyTargetPercent = 0;
        enemyTargetDamage = 0;

        //Oscar displays
        oscarTipsUI.SetActive(false);

        //Enemy displays
        enemyTipsUI.SetActive(false);
    }
}
