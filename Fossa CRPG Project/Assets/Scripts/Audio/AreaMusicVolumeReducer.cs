using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicVolumeReducer : MonoBehaviour
{
    public bool pauseOnCombat;

    //Min effect is how close you need to be before sound starts reducing
    //Max effect is how close you need to be before sound reaches 0
    public float MinEffectDistance = 10;
    public float MaxEffectDistance = 5;

    private GameObject player;
    private bool playerInArea = false;
    private float oldMusicValue;

    private bool inCombat;
    private float combatStartLerp;
    private float t;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    private void Update()
    {
        if (!playerInArea) { return; }
        if (inCombat)
        {
            if (t < 1)
            {
                AudioManager.instance.musicVolume = Mathf.Lerp(combatStartLerp, oldMusicValue, t / 1);
                t += Time.deltaTime;
            }
        }
        else
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < MinEffectDistance)
            {
                float percentage = ((dist - MaxEffectDistance) / (MinEffectDistance - MaxEffectDistance));
                if (percentage < 0) { percentage = 0; }

                AudioManager.instance.musicVolume = percentage;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
            oldMusicValue = AudioManager.instance.musicVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
            AudioManager.instance.musicVolume = oldMusicValue;
        }
    }
    private void StartCombat(string combatName)
    {
        if (!pauseOnCombat) { return; }

        t = 0;
        combatStartLerp = AudioManager.instance.musicVolume;
        inCombat = true;
    }

    private void EndCombat(string combatName)
    {
        if (!pauseOnCombat) { return; }
        inCombat = false;
    }
}
