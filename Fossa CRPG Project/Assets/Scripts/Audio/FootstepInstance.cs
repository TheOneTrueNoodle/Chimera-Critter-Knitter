using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FootstepInstance : MonoBehaviour
{
    public EventInstance playerFootsteps;

    private void Start()
    {
        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.footsteps);
    }
    public void UpdateSound(bool playing, float velocity)
    {
        if (playing)
        {
            PLAYBACK_STATE playbackstate;
            playerFootsteps.getPlaybackState(out playbackstate);
            if (playbackstate.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }

            playerFootsteps.setParameterByName("Velocity", velocity);
        }
        else
        {
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
