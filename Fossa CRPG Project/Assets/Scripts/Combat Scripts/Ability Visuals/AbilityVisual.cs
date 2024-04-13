using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVisual : MonoBehaviour
{
    public Entity targetUnit;

    public void Setup(Entity target)
    {
        targetUnit = target;
    }
}
