using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTriggerDisable : MonoBehaviour
{
    [SerializeField] private GameObject[] obj;

    public void OnTriggerEnter(Collider col)
    {
        foreach(GameObject obej in obj)
        {
            obej.SetActive(false);
        }
    }
    public void OnTriggerExit(Collider col)
    {
        foreach (GameObject obej in obj)
        {
            obej.SetActive(true);
        }
    }
}
