using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FilmGrainCam : MonoBehaviour
{
    [SerializeField] private PostProcessVolume filmGrainPPV;
    [SerializeField] private float targetWeight;

    private void Update()
    {
        if (PlayerPrefs.GetInt("Remove Grain") == 1)
        {
            //Remove grain
            filmGrainPPV.weight = 0;
        }
        else
        {
            filmGrainPPV.weight = targetWeight;
        }
    }
}
