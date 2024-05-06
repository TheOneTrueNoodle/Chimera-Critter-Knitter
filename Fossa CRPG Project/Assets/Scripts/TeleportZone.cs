using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public Transform targetPosition;
    public Animator anim;

    public GameObject objectToLoad;
    public GameObject objectToUnload;

    public void Call()
    {
        anim.Play("Transition");
    }

    public void Teleport()
    {
        GameObject Oscar = FindObjectOfType<PlayerMovement>().gameObject;
        Oscar.transform.position = targetPosition.position;

        Oscar.GetComponent<HeldItem>().currentAreaParent = objectToLoad.transform;

        objectToLoad.SetActive(true);
        objectToUnload.SetActive(false);
    }
}
