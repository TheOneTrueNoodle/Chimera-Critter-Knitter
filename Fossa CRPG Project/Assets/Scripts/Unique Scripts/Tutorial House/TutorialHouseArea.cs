using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TutorialHouseArea : AreaManager
{
    public PostProcessVolume infectionVignette;
    public FadeOutDogVision dogVision;
    public AbilityData armAbility;
    public GameObject muffinByTheDoor;
    public GameObject muffinReleaseScene;
    public GameObject muffinBasementDialogues;

    [SerializeField] public GameObject revealDialogue;
    [SerializeField] public GameObject postFightDialogue;

    [Header("Toilet")]
    [SerializeField] private GameObject toilet;
    [SerializeField] private FMODUnity.EventReference flushToilet;

    [Header("Tutorial Combat")]
    [SerializeField] private GameObject preFrog;
    [SerializeField] private GameObject postFrog;
    [SerializeField] private GameObject ignoredMuffin;

    [SerializeField] private AbilityData FrogTongueAbility;

    [Header("Boss fight")]
    [SerializeField] private GameObject bossCutsceneSecondPart;

    private void Start()
    {
        CombatEvents.current.onEndCombat += EndCombat;
        CombatEvents.current.onStartCombatSetup += StartCombat;

        DialogueEvents.current.onEndDialogue += EndDialogue;
    }

    private void Update()
    {
        if (areaBools["hasBlanket"])
        {
            areaBools["freedMuffin"] = true;
        }
        if (areaBools["freedMuffin"])
        {
            muffinByTheDoor.SetActive(true);
            muffinReleaseScene.SetActive(true);
            muffinBasementDialogues.SetActive(false);
        }
        if (areaBools["ignoredMuffin"])
        {
            ignoredMuffin.SetActive(true);
        }

    }

    private void OscarGetsHitForTheFirstTime(Entity attacker, Entity target)
    {
        if (target.CharacterData.Name == "Oscar")
        {
            //Trigger the cool infection vignette for the first time!
            if (PlayerPrefs.GetInt("Infection Effect") != 1) { StartCoroutine(AnimateInfectionVignette()); }
            CombatEvents.current.onAttackAttempt -= OscarGetsHitForTheFirstTime;
        }
    }

    public void CollectGateKey()
    {
        if (areaBools.ContainsKey("Has gate key"))
        {
            areaBools["Has gate key"] = true;
            Debug.Log("Has house key? " + areaBools["Has gate key"]);
        }
    }

    public void DevelopMutations()
    {
        if (areaBools.ContainsKey("Has mutated"))
        {
            areaBools["Has mutated"] = true;
        }
    }

    public void HasBlanket()
    {
        if (areaBools.ContainsKey("hasBlanket"))
        {
            areaBools["hasBlanket"] = true;
            
        }
    }

    public void BlanketBribed()
    {
        if (areaBools.ContainsKey("blanketBribed"))
        {
            areaBools["hasBlanket"] = true;
            areaBools["blanketBribed"] = true;
        }
    }

    public void IgnoredMuffin()
    {
        if (areaBools.ContainsKey("ignoredMuffin"))
        {
            areaBools["ignoredMuffin"] = true;
        }
    }

    public void freedMuffin()
    {
        if (areaBools.ContainsKey("freedMuffin"))
        {
            areaBools["freedMuffin"] = true;
        }
    }


    public void FlushToilet()
    {
        if (areaBools.ContainsKey("flushToilet"))
        {
            areaBools["flushToilet"] = true;
        }
    }

    private void EndDialogue()
    {
        if (areaBools.ContainsKey("BossFightCutscenePT2") && areaBools["BossFightCutscenePT2"])
        {
            //Turn on second cutscene
            bossCutsceneSecondPart.SetActive(true);
        }
        else if (areaBools.ContainsKey("flushToilet") && areaBools["flushToilet"])
        {
            areaBools["flushToilet"] = false;
            AudioManager.instance.PlayOneShot(flushToilet, toilet.transform.position);
        }
    }

    #region Combat Related Function
    private void StartCombat(string combatName)
    {
        if (combatName == "Tutorial_Combat")
        {
            CombatEvents.current.onAttackAttempt += OscarGetsHitForTheFirstTime;
        }
    }

    private void EndCombat(string combatName)
    {
        if (areaBools.ContainsKey("InCombat"))
        {
            areaBools["InCombat"] = false;
        }

        //Trigger for Tutorial Combat
        if (combatName == "Tutorial_Combat")
        {
            CombatEvents.current.onAttackAttempt -= OscarGetsHitForTheFirstTime;

            //Run code for after the first combat
            if (areaBools.ContainsKey("Infected"))
            {
                areaBools["Infected"] = true;
            }

            //Turn on post frog and turn off pre frog
            preFrog.SetActive(false);
            postFrog.SetActive(true);

            //Grant the player the mutation menu now :D
            MenuEvent.current.UnlockNewMutation(FrogTongueAbility);

            return;
        }

        //Trigger for Owner Combat
        if (combatName == "Owner")
        {
            MenuEvent.current.UnlockNewMutation(armAbility);

            if (areaBools.ContainsKey("defeatedOwner"))
            {
                areaBools["defeatedOwner"] = true;
                revealDialogue.SetActive(true);
                postFightDialogue.SetActive(true);
            }

            if (areaFloats.ContainsKey("Current Song"))
            {
                areaFloats["Current Song"] = 5;
            }

            AudioManager.instance.SetMusicSong(areaFloats["Current Song"]);

            return;
        }
    }

    private IEnumerator AnimateInfectionVignette()
    {
        float lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 2;
            float newValue = Mathf.Lerp(0f, 1, lerpTime);
            //Also lerp through an FMOD variable that muffles all audio during this scene
            infectionVignette.weight = newValue;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        dogVision.Call();
        lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 2;
            float newValue = Mathf.Lerp(1, 0, lerpTime);
            //Also lerp through an FMOD variable that muffles all audio during this scene
            infectionVignette.weight = newValue;
            yield return null;
        }
    }
    #endregion
}

