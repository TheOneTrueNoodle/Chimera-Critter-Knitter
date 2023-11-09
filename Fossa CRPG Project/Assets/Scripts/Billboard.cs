using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main is { }) transform.LookAt(-Camera.main.transform.position, Camera.main.transform.up);
    }
}
