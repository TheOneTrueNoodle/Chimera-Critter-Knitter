using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AmbientSound : MonoBehaviour
{
    public bool callOnStart;
    public EventReference sound;
    public FMOD.Studio.STOP_MODE stopMode;
    public bool pauseOnCombat;

    private EventInstance soundInstance;

    private bool called;

    private void Start()
    {
        if (callOnStart)
        {
            Call();
        }

        CombatEvents.current.onStartCombatSetup += startCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    public void Call()
    {
        if (called) { return; }

        called = true;
        soundInstance = AudioManager.instance.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
        soundInstance.start();
    }
    
    public void Stop()
    {
        if(!called) { return; }

        called = false;
        soundInstance.stop(stopMode);
    }

    private void startCombat(string combatName)
    {
        if (!pauseOnCombat) { return; }

        if (called) { soundInstance.stop(stopMode); }
    }

    private void EndCombat(string combatName)
    {
        if (!pauseOnCombat) { return; }
        if (called) { soundInstance.start(); }
    }
}
