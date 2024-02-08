using System;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public static InputEvents current;

    private void Awake()
    {
        current = this;
    }

}
