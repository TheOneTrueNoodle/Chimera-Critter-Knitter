using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullFoodDown_TutorialHouseArea : MonoBehaviour
{
    [SerializeField] private Animator foodFallingAnim;
    [SerializeField] private GameObject eatFoodInteraction;

    public void Call()
    {
        if (AreaManager.current.GetComponent<TutorialHouseArea>().areaBools.ContainsKey("Has mutated") && AreaManager.current.GetComponent<TutorialHouseArea>().areaBools["Has mutated"] == true)
        {
            foodFallingAnim.enabled = true;
            foodFallingAnim.Play("DogFoodFall");
            eatFoodInteraction.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
