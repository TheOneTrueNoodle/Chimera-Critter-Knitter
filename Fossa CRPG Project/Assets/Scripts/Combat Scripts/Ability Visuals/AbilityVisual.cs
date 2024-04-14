using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVisual : MonoBehaviour
{
    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public Transform abilitySource;

    private void Start()
    {
        Call();
    }

    public void Setup(Vector3 targetPos, Transform source)
    {
        targetPosition = targetPos;
        abilitySource = source;
        Call();
    }

    public virtual void Call()
    {

    }
}
