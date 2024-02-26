using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObjectDownStairs_TutorialHouseArea : MonoBehaviour
{
    [SerializeField] private GameObject barricadeBroken;
    [SerializeField] private GameObject barricadeFixed;
    [SerializeField] private GameObject ownerCombatTrigger;
    [SerializeField] private GameObject ownerAtDoor;
    [SerializeField] private GameObject ownerEnemy;

    [SerializeField] private Animator objectAnim;

    public void Call()
    {
        if (AreaManager.current.GetComponent<TutorialHouseArea>().areaBools.ContainsKey("Has mutated") && AreaManager.current.GetComponent<TutorialHouseArea>().areaBools["Has mutated"] == true)
        {
            barricadeBroken.SetActive(true);
            barricadeFixed.SetActive(false);
            ownerCombatTrigger.SetActive(true);
            ownerAtDoor.SetActive(false);
            ownerEnemy.SetActive(true);
            objectAnim.enabled = true;

            gameObject.SetActive(false);
        }
    }
}
