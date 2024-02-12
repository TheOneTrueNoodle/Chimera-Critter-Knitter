using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarHasKey : MonoBehaviour
{

    public bool HasUpstairsKey;
    public bool gateOpen;
    public Animator animator;
    public GameObject Gate;


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

    }

}
