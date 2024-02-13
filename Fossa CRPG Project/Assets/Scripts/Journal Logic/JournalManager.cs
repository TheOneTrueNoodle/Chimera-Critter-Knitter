using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public List<JournalEntryButton> entries;
    [SerializeField] private TMP_Text descriptionText;

    private void Start()
    {
        Debug.Log(JournalEvent.current);
        JournalEvent.current.onFindEntryObject += DiscoverEntry;
        JournalEvent.current.onEntrySelect += EntrySelect;

        var buttons = GetComponentsInChildren<JournalEntryButton>(true);

        foreach (var entry in buttons)
        {
            entries.Add(entry);
        }
    }

    public void DiscoverEntry(JournalEntryData data)
    {
        foreach (var entry in entries)
        {
            if (entry.entryData.entryName == data.entryName)
            {
                if (entry.discovered)
                {
                    return;
                }
                else
                {
                    entry.Discover();
                    entry.discovered = true;

                    return;
                }
            }
        }
    }

    public void EntrySelect(JournalEntryData data, bool discovered)
    {
        if (discovered)
        {
            descriptionText.text = data.description;
        }
        else
        {
            descriptionText.text = "???";
        }
    }
}
