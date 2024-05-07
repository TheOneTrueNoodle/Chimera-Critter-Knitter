using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Flashback : MonoBehaviour
{
    [SerializeField] private PostProcessVolume ppVolume;
    [SerializeField] private Animator anim;

    [SerializeField] private bool disableOnFinish;

    public void Call()
    {
        StartCoroutine(FadeInVolume());
        anim.Play("Fade In");

        DialogueEvents.current.onEndDialogue += EndDialogue;
    }

    private void EndDialogue()
    {
        StartCoroutine(FadeOutVolume());
        anim.Play("Fade Out");

        DialogueEvents.current.onEndDialogue -= EndDialogue;
        if (disableOnFinish) { gameObject.SetActive(false); }
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

        ppVolume.weight = 0;
    }
    private IEnumerator FadeInVolume()
    {
        float lerpTimer = 0;
        while (lerpTimer < 1)
        {
            lerpTimer += Time.deltaTime;
            float newValue = Mathf.Lerp(0, 1, lerpTimer);
            ppVolume.weight = newValue;
            yield return null;
        }

        ppVolume.weight = 1;
    }
}
