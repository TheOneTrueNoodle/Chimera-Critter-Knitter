using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSongParameter : MonoBehaviour
{
    public string parameterName;
    public float parameterValue;
    public void Call()
    {
        AudioManager.instance.SetSongParameter(parameterName, parameterValue);
    }
}
