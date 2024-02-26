using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public Transform targetPosition;

    public void Call()
    {
        GameObject Oscar = FindObjectOfType<PlayerMovement>().gameObject;
        Oscar.transform.position = targetPosition.position;
    }
}
