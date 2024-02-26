using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    //This is a general class for all areas in teh game. Other areas will derive from this class making a specific areas script searchable by just finding the local Area Manager. 
    public Dictionary<string, bool> areaBools;
    public Dictionary<string, float> areaFloats;

    public static AreaManager current;

    private void Awake()
    {
        current = this;
    }
}
