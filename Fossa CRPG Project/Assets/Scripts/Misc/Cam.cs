using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] public GameObject teleporttTo;
    [SerializeField] public GameObject teleporte;
    [SerializeField] public GameObject playerr;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject == playerr)
        {
            teleporte.transform.position = teleporttTo.transform.position;
        }
    }
}
