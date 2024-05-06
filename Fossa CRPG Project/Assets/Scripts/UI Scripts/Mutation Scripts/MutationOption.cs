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

    [field: SerializeField] public FMODUnity.EventReference audioEvent;

    private MutationSlot currentSlot;
    [HideInInspector] public bool equipped;

    public void Setup(AbilityData ability)
    {
        mutationAbility = ability;
        image.sprite = ability.symbol;
        nameDisp.text = ability.Name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(audioEvent, transform.position);

        MenuEvent.current.TryEquipMutation(this);
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
