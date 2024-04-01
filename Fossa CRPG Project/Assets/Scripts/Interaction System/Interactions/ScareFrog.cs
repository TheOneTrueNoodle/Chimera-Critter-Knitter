using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareFrog : MonoBehaviour
{
    public void Call()
    {
        var anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.Play("Jump");
            var rb = GetComponent<Rigidbody>();
            rb.AddForce((transform.forward + Vector3.up) * 10, ForceMode.Impulse);
        }
    }
}
