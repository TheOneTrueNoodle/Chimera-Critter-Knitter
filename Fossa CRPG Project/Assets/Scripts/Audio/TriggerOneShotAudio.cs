using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOneShotAudio : MonoBehaviour
{
    [field: SerializeField] public FMODUnity.EventReference audioEvent;

    [SerializeField] private Transform AudioOrigin;
    
    public void Call()
    {
        if(AudioOrigin == null) { AudioOrigin = transform; }
        AudioManager.instance.PlayOneShot(audioEvent, AudioOrigin.position);
    }
}
