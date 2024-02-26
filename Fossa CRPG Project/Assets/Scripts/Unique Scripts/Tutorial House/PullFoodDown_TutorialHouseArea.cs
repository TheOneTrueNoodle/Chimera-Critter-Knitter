using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullFoodDown_TutorialHouseArea : MonoBehaviour
{
    public Animator foodFallingAnim;
    public GameObject sparkles;

    public void Call()
    {
        if (AreaManager.current.GetComponent<TutorialHouseArea>().areaBools.ContainsKey("Has mutated") && AreaManager.current.GetComponent<TutorialHouseArea>().areaBools["Has mutated"] == true)
        {
            foodFallingAnim.enabled = true;
            foodFallingAnim.Play("DogFoodFall");
            sparkles.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
