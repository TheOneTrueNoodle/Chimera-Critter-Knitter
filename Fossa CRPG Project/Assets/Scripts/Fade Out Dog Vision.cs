using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FadeOutDogVision : MonoBehaviour
{
    public PostProcessVolume ppVolume;
    private bool disabled = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Dog Vision") == 1)
        {
            ppVolume.weight = 0;
            disabled = true;
        }
        else
        {
            ppVolume.weight = 1;
            disabled = true;
        }
    }

    public void Call()
    {
        if (disabled) { return; }
        StartCoroutine(FadeOutVolume());
    }

    private IEnumerator FadeOutVolume()
    {
        float lerpTimer = 0;
        while (lerpTimer < 1)
        {
            lerpTimer += Time.deltaTime;
            float newValue = Mathf.Lerp(1, 0, lerpTimer);
            ppVolume.weight = newValue;
            yield return null;
        }
    }
}
