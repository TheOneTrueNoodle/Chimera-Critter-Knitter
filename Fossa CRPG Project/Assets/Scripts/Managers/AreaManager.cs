using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    //This is a general class for all areas in teh game. Other areas will derive from this class making a specific areas script searchable by just finding the local Area Manager. 
    public List<AreaBool> assignedAreaBools;
    public List<AreaFloat> assignedAreaFloats;
    public Dictionary<string, bool> areaBools;
    public Dictionary<string, float> areaFloats;

    public static AreaManager current;

    private void Awake()
    {
        current = this;
        if(assignedAreaBools != null && assignedAreaBools.Count > 0)
        {
            areaBools = new Dictionary<string, bool>();
            foreach (AreaBool areaBool in assignedAreaBools)
            {
                areaBools.Add(areaBool.boolName, areaBool.value);
            }
        }
        if (assignedAreaFloats != null && assignedAreaFloats.Count > 0)
        {
            areaFloats = new Dictionary<string, float>();
            foreach (AreaFloat areaFloat in assignedAreaFloats)
            {
                areaFloats.Add(areaFloat.floatName, areaFloat.value);
            }
        }
    }
}

[System.Serializable]
public class AreaBool
{
    public string boolName;
    public bool value;
}

[System.Serializable]
public class AreaFloat
{
    public string floatName;
    public float value;
}