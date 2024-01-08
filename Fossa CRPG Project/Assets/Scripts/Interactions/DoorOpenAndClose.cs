using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DoorOpenAndClose : MonoBehaviour
{
    [field: SerializeField] public EventReference OpenSFX;
    [field: SerializeField] public EventReference CloseSFX;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        anim.Play("Door Open");
        AudioManager.instance.PlayOneShot(OpenSFX, transform.position);
    }

    public void CloseDoor()
    {
        anim.Play("Door Close");
        AudioManager.instance.PlayOneShot(CloseSFX, transform.position);
    }
}
