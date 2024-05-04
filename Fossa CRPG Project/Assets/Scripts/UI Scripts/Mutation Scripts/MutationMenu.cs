using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationMenu : MonoBehaviour
{
    private Entity oscarData;

    private List<MutationOption> createdMutations = new List<MutationOption>();

    [HideInInspector] public List<AbilityData> unlockedAbilities = new List<AbilityData>();
    private List<AbilityData> setupAbilties = new List<AbilityData>();

    public List<AbilityData> equippedMutations;

    [Header("Mutation Displays")]
    [SerializeField] private Transform mutationParent;
    [SerializeField] private GameObject mutationOptionPrefab;

    private void Start()
    {
        MenuEvent.current.onUnlockNewMutation += UnlockNewMutation;
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
