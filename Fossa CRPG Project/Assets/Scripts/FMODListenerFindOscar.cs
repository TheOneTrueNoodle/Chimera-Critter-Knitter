using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODListenerFindOscar : MonoBehaviour
{
    void Start()
    {
        GetComponent<StudioListener>().attenuationObject = FindObjectOfType<PlayerMovement>().gameObject;
    }
}
