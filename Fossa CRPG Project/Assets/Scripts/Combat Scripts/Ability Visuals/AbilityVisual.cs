using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVisual : MonoBehaviour
{
    public GameObject targetUnit;
    public GameObject abilitySource;

    private void Start()
    {
        Call();
    }

    public void Setup(Entity target, Entity source)
    {
        targetUnit = target.gameObject;
        abilitySource = source.gameObject;
        Call();
    }

    public virtual void Call()
    {

    }
}
