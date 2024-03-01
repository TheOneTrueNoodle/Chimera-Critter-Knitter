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
}
