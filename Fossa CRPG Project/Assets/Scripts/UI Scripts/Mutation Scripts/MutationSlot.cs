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
    public Image hasMutationDisp;

    public bool hasEquippedMutation;

    public void EquipMutation(MutationOption mutation)
    {
        currentSelectedMutation = mutation;
        mutationImage.sprite = currentSelectedMutation.mutationAbility.symbol;
        nameDisp.text = currentSelectedMutation.mutationAbility.Name;
        hasMutationDisp.enabled = true;
        hasEquippedMutation = true;
        MenuEvent.current.EquipMutation(currentSelectedMutation.mutationAbility);
    }

    public void UnequipMutation()
    {
        MenuEvent.current.UnequipMutation(currentSelectedMutation.mutationAbility);
        currentSelectedMutation.Unequip();

        currentSelectedMutation = null;
        mutationImage.sprite = null;
        nameDisp.text = "";
        hasMutationDisp.enabled = false;
        hasEquippedMutation = false;
    }
}
