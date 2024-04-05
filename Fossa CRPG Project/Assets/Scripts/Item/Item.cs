using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector] public ItemType itemType;
    public string itemName;

    public Vector3 holdRotationV3;
    public Rigidbody rb;
    public Collider physicsCollider;
    public Collider triggerCollider;

    public void enablePhysics()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
    }
    public void disablePhysics()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
