using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvent : MonoBehaviour
{
    public static MenuEvent current;

    private void Awake()
    {
        current = this;
    }

    public event Action<JournalEntryData> onFindEntryObject;
    public void FindEntryObject(JournalEntryData data)
    {
        if (onFindEntryObject != null)
        {
            onFindEntryObject(data);
        }
    }

    public event Action<JournalEntryData, bool> onEntrySelect;
    public void EntrySelect(JournalEntryData data, bool discovered)
    {
        if (onEntrySelect != null)
        {
            onEntrySelect(data, discovered);
        }
    }

    public event Action<AbilityData> onUnlockNewMutation;
    public void UnlockNewMutation(AbilityData mutation)
    {
        if (onUnlockNewMutation != null)
        {
            onUnlockNewMutation(mutation);
        }
    }

    public event Action<MutationOption> onTryEquipMutation;
    public void TryEquipMutation(MutationOption option)
    {
        if (onTryEquipMutation != null)
        {
            onTryEquipMutation(option);
        }
    }

    public event Action<AbilityData> onEquipMutation;
    public void EquipMutation(AbilityData mutation)
    {
        if (onEquipMutation != null)
        {
            onEquipMutation(mutation);
        }
    }

    public event Action<AbilityData> onUnequipMutation;
    public void UnequipMutation(AbilityData mutation)
    {
        if (onUnequipMutation != null)
        {
            onUnequipMutation(mutation);
        }
    }

    public event Action<string> onSpawnPopup;
    public void SpawnPopup(string text)
    {
        if (onSpawnPopup != null)
        {
            onSpawnPopup(text);
        }
    }

    public event Action<Interactable> onShowInteractUI;
    public void ShowInteractUI(Interactable interactable)
    {
        if (onShowInteractUI != null)
        {
            onShowInteractUI(interactable);
        }
    }

    public event Action onHideInteractUI;
    public void HideInteractUI()
    {
        if (onHideInteractUI != null)
        {
            onHideInteractUI();
        }
    }

    public event Action onOpenMenu;
    public void OpenMenu()
    {
        if (onOpenMenu != null)
        {
            onOpenMenu();
        }
    }

    public event Action onCloseMenu;
    public void CloseMenu()
    {
        if (onCloseMenu != null)
        {
            onCloseMenu();
        }
    }
}
