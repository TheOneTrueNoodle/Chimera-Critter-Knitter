using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationScreenEffect : MonoBehaviour
{

    public GameObject mutationScreenEffect;

    void OnTriggerEnter(Collider Player)
    {
    
        mutationScreenEffect.SetActive(true);
    }

    void OnTriggerExit(Collider Player)
    {
        mutationScreenEffect.SetActive(false);
    }
}
