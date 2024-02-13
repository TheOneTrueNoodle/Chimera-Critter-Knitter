using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarHasKey : MonoBehaviour
{

    public bool HasUpstairsKey;
    public bool gateOpen;
    public Animator animator;
    public Animator FoodFall;
    public Animator Lick;
    public Animator Lick2;
    public GameObject Gate;
    public GameObject mutationSpot1;
    public GameObject mutationSpot2;
    public GameObject DogFood;
    public GameObject Oscar;
    public GameObject ScreenEffect;
    public GameObject dogfoodsparkles;
    public GameObject barricadeFixed;
    public GameObject barricadeBroken;
    public GameObject Ownerhome;
    public GameObject OwnerhomeCutscene;


    void Awake()
    {
      
        HasUpstairsKey = false;
        gateOpen = false;
     

    }
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "UpstairsKey")
        {
            HasUpstairsKey = true;
        }
        if ((other.tag == "UpstairsGate") && (HasUpstairsKey == true))
        {
            gateOpen = true;
            animator.enabled = true;

        }
        if (other.tag == "NoFood")
        {
            mutationSpot1.SetActive(true);
            mutationSpot2.SetActive(true);
            Destroy(other.gameObject);

        }
        if (other.tag == "FirstMutationSpot")
        {
            ScreenEffect.SetActive(true);
        }
   



    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "FirstMutationSpot")
        {
            ScreenEffect.SetActive(false);
        }
        if (other.tag == "SecondMutationSpot")
        {
            ScreenEffect.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.tag == "FirstMutationSpot") && (Input.GetKeyDown(KeyCode.E)))
        {
            Lick.enabled = true;
            FoodFall.enabled = true;
            mutationSpot1.SetActive(false);
            ScreenEffect.SetActive(false);
            dogfoodsparkles.SetActive(false);

        }
        if ((other.tag == "SecondMutationSpot") && (Input.GetKeyDown(KeyCode.E)))
        {
            Lick.enabled = true;
            Lick2.enabled = true;
            mutationSpot2.SetActive(false);
            ScreenEffect.SetActive(false);
            barricadeBroken.SetActive(true);
            barricadeFixed.SetActive(false);
            Ownerhome.SetActive(true);

        }
    }


}
