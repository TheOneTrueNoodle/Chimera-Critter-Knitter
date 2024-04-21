using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public bool inCombat;
    [HideInInspector] public bool inDialogue;
    private bool initializedEvents;

    public bool hasSmell;

    public bool oneTimeUse;

    [HideInInspector] public bool used = false;
    public InteractionEvent singleInteraction;

    [HideInInspector] public bool active;
    public InteractionEvent enableInteraction;
    public InteractionEvent disableInteraction;

    [SerializeField] private ParticleSystem smellParticles;

    public Sprite interactSprite;

    void Start()
    {
        initializedEvents = true;
        subscribeToEvents();

        Debug.Log("Has Smell: " + hasSmell + "InteractionManager.current: " + InteractionManager.current);

        if (hasSmell)
        {
            InteractionManager.current.interactables.Add(this);
        }
    }

    public virtual void CallInteraction()
    {

    }

    public void enableSmell()
    {
        if (!smellParticles.isPlaying)
        {
            smellParticles.Play();
        }
    }
    public void disableSmell()
    {
        smellParticles.Stop();
    }

    #region Event Calls
    private void StartCombat(string combatName)
    {
        inCombat = true;
    }
    private void EndCombat(string combatName)
    {
        inCombat = false;
    }
    private void StartDialogue()
    {
        inDialogue = true;
    }
    private void EndDialogue()
    {
        inDialogue = false;
    }
    private void OnEnable()
    {
        if (initializedEvents) { subscribeToEvents(); }
    }
    private void OnDisable()
    {
        unsubscribeToEvents();
    }
    private void OnDestroy()
    {
        unsubscribeToEvents();
    }

    private void subscribeToEvents()
    {
        CombatEvents.current.onStartCombatSetup += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;

        DialogueEvents.current.onStartDialogue += StartDialogue;
        DialogueEvents.current.onEndDialogue += EndDialogue;
    }
    private void unsubscribeToEvents()
    {
        CombatEvents.current.onStartCombatSetup -= StartCombat;
        CombatEvents.current.onEndCombat -= EndCombat;

        DialogueEvents.current.onStartDialogue -= StartDialogue;
        DialogueEvents.current.onEndDialogue -= EndDialogue;
    }
    #endregion
}

[Serializable]
public class InteractionEvent : UnityEvent { }
