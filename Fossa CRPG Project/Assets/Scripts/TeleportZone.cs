using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public Transform targetPosition;
    public Animator anim;

    public void Call()
    {
        anim.Play("Transition");
    }

    public void Teleport()
    {
        GameObject Oscar = FindObjectOfType<PlayerMovement>().gameObject;
        Oscar.transform.position = targetPosition.position;
    }
}
