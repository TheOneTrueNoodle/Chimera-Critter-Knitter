using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference areaAmbience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference areaMusic { get; private set; }
    [field: SerializeField] public EventReference victoryMusic { get; private set; }

    [field: Header("Oscar SFX")]
    [field: SerializeField] public EventReference oscarBark { get; private set; }
    [field: SerializeField] public EventReference oscarSmell { get; private set; }

    [field: Header("Unit SFX")]
    [field: SerializeField] public EventReference xpGainSFX { get; private set; }
    [field: SerializeField] public EventReference levelUpSFX { get; private set; }
    [field: SerializeField] public EventReference footsteps { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
