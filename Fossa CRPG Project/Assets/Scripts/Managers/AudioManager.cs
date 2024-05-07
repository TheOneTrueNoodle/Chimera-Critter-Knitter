using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)] public float masterVolume = 1;
    [Range(0, 1)] public float musicVolume = 1;
    [Range(0, 1)] public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus SFXBus;

    private List<EventInstance> eventInstances;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        if (!FMODEvents.instance.areaAmbience.IsNull) { InitializeAmbience(FMODEvents.instance.areaAmbience); }
        if (!FMODEvents.instance.areaMusic.IsNull) { InitializeMusic(FMODEvents.instance.areaMusic); }
    }

    private void Update()
    {
        SetAudioVolume();
    }

    public void SetAudioVolume()
    {
        masterBus.setVolume(PlayerPrefs.GetFloat("Master Volume", 1));
        musicBus.setVolume(PlayerPrefs.GetFloat("Music Volume", 1));
        SFXBus.setVolume(PlayerPrefs.GetFloat("SFX Volume", 1));
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }
    public float GetCurrentSong()
    {
        float currentSong;
        musicEventInstance.getParameterByName("Song", out currentSong);
        return currentSong;
    }
    public void SetMusicSong(float Song)
    {
        musicEventInstance.setParameterByName("Song", Song);
        Debug.Log("Changing song supposedly to this number: " + Song);
    }
    public void SetSongParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos); 
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
