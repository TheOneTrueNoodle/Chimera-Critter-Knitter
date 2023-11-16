using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] public GameObject teleportTo;
    [SerializeField] public GameObject teleportee;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject == teleportee)
        {
            other.transform.position = teleportTo.transform.position;
        }
    }
}
