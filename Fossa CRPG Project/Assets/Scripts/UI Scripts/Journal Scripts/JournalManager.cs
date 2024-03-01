using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public List<JournalEntryData> entries;
    private List<JournalEntryButton> buttons;
    [SerializeField] private JournalEntryButton entryButtonPrefab;
    [SerializeField] private Transform entryButtonParent;
    [SerializeField] private TMP_Text descriptionText;

    private void Start()
    {
        MenuEvent.current.onFindEntryObject += DiscoverEntry;
        MenuEvent.current.onEntrySelect += EntrySelect;

        buttons = new List<JournalEntryButton>();

        foreach (var entry in entries)
        {
            var entryButton = Instantiate(entryButtonPrefab, entryButtonParent);
            buttons.Add(entryButton);
            entryButton.entryData = entry;
        }
    }

    public void DiscoverEntry(JournalEntryData data)
    {
        foreach (var entry in buttons)
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
