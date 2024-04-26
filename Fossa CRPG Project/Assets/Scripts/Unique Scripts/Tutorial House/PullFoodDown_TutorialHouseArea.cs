using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullFoodDown_TutorialHouseArea : MonoBehaviour
{
    [SerializeField] private GameObject eatFoodInteraction;

    public void Call()
    {
        if (AreaManager.current.GetComponent<TutorialHouseArea>().areaBools.ContainsKey("Has mutated") && AreaManager.current.GetComponent<TutorialHouseArea>().areaBools["Has mutated"] == true)
        {
            eatFoodInteraction.SetActive(true);

            MenuEvent.current.SpawnPopup("Eat some food to heal!");

            gameObject.SetActive(false);
        }
    }
}
