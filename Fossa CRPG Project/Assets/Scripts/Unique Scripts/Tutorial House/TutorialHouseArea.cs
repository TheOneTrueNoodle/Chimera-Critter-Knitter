using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TutorialHouseArea : AreaManager
{
    public PostProcessVolume infectionVignette;
    public FadeOutDogVision dogVision;
    public AbilityData lickAbility;
    public AbilityData armAbility;

    private void Start()
    {
        CombatEvents.current.onEndCombat += EndCombat;
        CombatEvents.current.onStartCombatSetup += StartCombat;
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
            //Grant the player the mutation menu now :D
            MenuEvent.current.UnlockNewMutation(lickAbility);
        }
    }

    public void HasBlanket()
    {
        if (areaBools.ContainsKey("hasBlanket"))
        {
            areaBools["hasBlanket"] = true;
            
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
        //Trigger for Tutorial Combat
        if (combatName == "Tutorial_Combat")
        {
            CombatEvents.current.onAttackAttempt -= OscarGetsHitForTheFirstTime;
            //Run code for after the first combat
            if (areaBools.ContainsKey("Infected"))
            {
                areaBools["Infected"] = true;
            }
            return;
        }
        //Trigger for Owner Combat
        if (combatName == "Owner")
        {
            MenuEvent.current.UnlockNewMutation(armAbility);
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

