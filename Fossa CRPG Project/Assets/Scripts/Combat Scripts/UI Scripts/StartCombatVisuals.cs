using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCombatVisuals : MonoBehaviour
{
    public Animator anim;

    [SerializeField] private Sprite frogBackground;
    [SerializeField] private Sprite bossBackground;

    [SerializeField] private Sprite frogGreen;
    [SerializeField] private Sprite frogBlue;
    [SerializeField] private Sprite boss;

    [SerializeField] private Image opponentBackgroundImage;
    [SerializeField] private Image opponentImageOne;
    [SerializeField] private Image opponentImageTwo;

    public void ChangeImages(string CombatName)
    {
        switch (CombatName)
        {
            case "Tutorial_Combat":
                opponentBackgroundImage.sprite = frogBackground;
                opponentImageOne.sprite = frogGreen;
                opponentImageTwo.gameObject.SetActive(false);
                break;
            case "Two Frogs":
                opponentBackgroundImage.sprite = frogBackground;
                opponentImageOne.sprite = frogGreen;
                opponentImageTwo.gameObject.SetActive(true);
                opponentImageTwo.sprite = frogBlue;
                break;
            case "Owner":
                opponentBackgroundImage.sprite = bossBackground;
                opponentImageOne.sprite = boss;
                opponentImageTwo.gameObject.SetActive(false);
                break;
        }

        anim.Play("New Start Combat");
    }
}
