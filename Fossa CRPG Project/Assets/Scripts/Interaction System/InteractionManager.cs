using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;
    private bool initializedEvents;

    public List<Interactable> interactables;

    private bool smellMode;
    private float smellTimer;
    public float smellDuration;

    public static InteractionManager current;

    [HideInInspector] public bool oscarHasPickedUpItemBefore;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        initializedEvents = true;
        subscribeToEvents();
    }

    private void Update()
    {
        if(inCombat || inDialogue) { return; }
        if (smellMode)
        {
            smellTimer += Time.deltaTime;
            if (smellTimer > smellDuration)
            {
                disableSmells();
            }
        }
        
        if(Input.GetButtonDown("Smell"))
        {
            //Now in smell mode
            enableSmells();
        }
    }

    public void PickupItem(PickupItem item)
    {
        if (!oscarHasPickedUpItemBefore)
        {
            oscarHasPickedUpItemBefore = true;
            if (AreaManager.current.GetComponent<TutorialHouseArea>() != null)
            {
                AreaManager.current.GetComponent<TutorialHouseArea>().HeldItemTutorial();
            }
        }

        if (item.journalEntry != null && !item.hasBeenPickedUp)
        {
            item.hasBeenPickedUp = true;
            MenuEvent.current.FindEntryObject(item.journalEntry);
        }
    }

    private void enableSmells()
    {
        smellMode = true;
        smellTimer = 0;

        foreach (Interactable interactable in interactables)
        {
            interactable.enableSmell();
        }
    }

    private void disableSmells()
    {
        smellMode = false;

        foreach (Interactable interactable in interactables)
        {
            interactable.disableSmell();
        }
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
