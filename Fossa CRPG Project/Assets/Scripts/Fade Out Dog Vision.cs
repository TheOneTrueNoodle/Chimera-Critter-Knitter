using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FadeOutDogVision : MonoBehaviour
{
    public PostProcessVolume ppVolume;

    public void Call()
    {
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
