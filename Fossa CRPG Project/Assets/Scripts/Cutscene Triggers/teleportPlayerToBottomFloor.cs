using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportPlayerToBottomFloor : MonoBehaviour
{
    public GameObject PlayerChar;
    public GameObject Downstairs;
    public GameObject Upstairs;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "UpToDown")
        {
            PlayerChar.transform.position = Downstairs.transform.position;
        }

        if (other.tag == "DownToUp")
        {
            PlayerChar.transform.position = Upstairs.transform.position;
        }
    }
    
}
