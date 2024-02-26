using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationScreenEffect : MonoBehaviour
{
    public GameObject mutationScreenEffect;
    public GameObject pressE;

    void OnTriggerEnter(Collider Player)
    {
        mutationScreenEffect.SetActive(true);
        pressE.SetActive(true);
    }

    void OnTriggerExit(Collider Player)
    {
        mutationScreenEffect.SetActive(false);
        pressE.SetActive(false);
    }
}
