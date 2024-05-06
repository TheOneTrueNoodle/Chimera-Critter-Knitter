using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerOneShotAudioOnHover : MonoBehaviour, IPointerEnterHandler
{
    [field: SerializeField] private FMODUnity.EventReference hoverSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(hoverSFX, transform.position);
    }
}
