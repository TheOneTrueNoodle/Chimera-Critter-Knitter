using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationMenu : MonoBehaviour
{
    private Entity oscarData;

    private List<MutationOption> createdMutations = new List<MutationOption>();

    [HideInInspector] public List<AbilityData> totalUnlockedAbilities = new List<AbilityData>();
    [HideInInspector] public List<AbilityData> unlockedAbilities = new List<AbilityData>();
    private List<AbilityData> setupAbilties = new List<AbilityData>();

    [HideInInspector] public List<AbilityData> equippedMutations;

    [Header("Mutation Displays")]
    public List<MutationSlot> mutationSlots;
    [SerializeField] private Transform mutationParent;
    [SerializeField] private GameObject mutationOptionPrefab;
    public GameObject exclamation;
    [SerializeField] private GameObject exclamation2;



    private void Start()
    {
        MenuEvent.current.onUnlockNewMutation += UnlockNewMutation;

        MenuEvent.current.onTryEquipMutation += TryEquipMutation;

        MenuEvent.current.onEquipMutation += EquipMutation;
        MenuEvent.current.onUnequipMutation += UnequipMutation;

        CombatEvents.current.onStartCombatSetup += StartCombat;
    }

    private void StartCombat(string combatName)
    {
        if (oscarData == null) { oscarData = FindObjectOfType<PlayerMovement>().GetComponent<Entity>(); }

        oscarData.activeAbilities.Clear();
        oscarData.activeAbilities = equippedMutations;
    }

    public void UpdateDisplay()
    {
        if(unlockedAbilities.Count == 0) { return; }
        foreach (var ability in unlockedAbilities)
        {
            bool duplicate = false;
            if (setupAbilties.Contains(ability))
            {
                duplicate = true;
            }

            if (!duplicate)
            {
                MutationOption newMutation = Instantiate(mutationOptionPrefab, mutationParent).GetComponent<MutationOption>();
                createdMutations.Add(newMutation);
                newMutation.Setup(ability);
                setupAbilties.Add(ability);
                totalUnlockedAbilities.Add(ability);
            }
        }
        unlockedAbilities.Clear();
    }

    public void UnlockNewMutation(AbilityData mutation)
    {
        //Check if we already have this mutation
        if(setupAbilties.Contains(mutation)) { return; }

        //Assign mutation if we dont have it
        unlockedAbilities.Add(mutation);
        MenuEvent.current.SpawnPopup("You gained a new Mutation!");
        exclamation.SetActive(true);
        exclamation2.SetActive(true);

    }

    public void TryEquipMutation(MutationOption optionSelected)
    {
        //Lets check all slots if they are free
        if (!optionSelected.equipped)
        {
            foreach (MutationSlot slot in mutationSlots)
            {
                if (!slot.hasEquippedMutation)
                {
                    //Equip Mutation
                    optionSelected.Equip(slot);
                    return;
                }
            }
            //No available slot code
        }
        else
        {
            //Unequip code
            optionSelected.Unequip();
        }
    }

    public void EquipMutation(AbilityData mutation)
    {
        if (setupAbilties.Contains(mutation) && !equippedMutations.Contains(mutation))
        {
            equippedMutations.Add(mutation);
        }
    }

    public void UnequipMutation(AbilityData mutation)
    {
        if (equippedMutations.Contains(mutation))
        {
            equippedMutations.Remove(mutation);
        }
    }
}
