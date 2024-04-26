using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MutationSlot : MonoBehaviour
{
    public MutationOption currentSelectedMutation;

    [Header("Displays")]
    [SerializeField] private Image mutationImage;
    [SerializeField] private TMP_Text nameDisp;

    public Image OscarVisualSlot;

    public bool hasEquippedMutation;

    public void EquipMutation(MutationOption mutation)
    {
        MenuEvent.current.EquipMutation(mutation.mutationAbility);
        currentSelectedMutation = mutation;
        mutationImage.enabled = true;
        mutationImage.sprite = currentSelectedMutation.mutationAbility.symbol;
        nameDisp.text = currentSelectedMutation.mutationAbility.Name;
        hasEquippedMutation = true;

        OscarVisualSlot.gameObject.SetActive(true);
        OscarVisualSlot.sprite = currentSelectedMutation.mutationAbility.mutationMenuVisual;
        OscarVisualSlot.preserveAspect = true;
    }

    public void UnequipMutation()
    {
        MenuEvent.current.UnequipMutation(currentSelectedMutation.mutationAbility);

        currentSelectedMutation = null;
        mutationImage.enabled = false;
        mutationImage.sprite = null;
        nameDisp.text = "";
        hasEquippedMutation = false;

        OscarVisualSlot.gameObject.SetActive(false);
        OscarVisualSlot.sprite = null;
    }
}
