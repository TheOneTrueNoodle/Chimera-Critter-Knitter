using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MutationOption : MonoBehaviour, IPointerDownHandler
{
    public AbilityData mutationAbility;
    
    [Header("Displays")]
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameDisp;
    [SerializeField] private Image equippedDisp;

    private MutationSlot currentSlot;
    private bool equipped;

    public void Setup(AbilityData ability)
    {
        mutationAbility = ability;
        image.sprite = ability.symbol;
        nameDisp.text = ability.Name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        if (!equipped)
        {
            MutationSlot[] slots = FindObjectsOfType<MutationSlot>();
            foreach (MutationSlot slot in slots)
            {
                if (!slot.hasEquippedMutation)
                {
                    //Equip Mutation
                    Equip(slot);
                    return;
                }
            }
            //No available slot code
        }
        else
        {
            //Unequip code
            Unequip();
        }
    }

    public void Equip(MutationSlot slot)
    {
        currentSlot = slot;
        slot.EquipMutation(this);
        equippedDisp.enabled = true;
        equipped = true;
    }

    public void Unequip()
    {
        currentSlot.UnequipMutation();
        currentSlot = null;
        equippedDisp.enabled = false;
        equipped = false;
    }
}
