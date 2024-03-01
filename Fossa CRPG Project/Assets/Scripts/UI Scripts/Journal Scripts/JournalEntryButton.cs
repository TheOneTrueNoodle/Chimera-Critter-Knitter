using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalEntryButton : MonoBehaviour
{
    [SerializeField] private GameObject undiscoveredShade;
    [SerializeField] private TMP_Text journalText;
    public JournalEntryData entryData;

    public bool discovered;

    private void Start()
    {
        if (!discovered)
        {
            undiscoveredShade.SetActive(true);
            journalText.text = "???";
        }
        else
        {
            undiscoveredShade.SetActive(false);
            journalText.text = entryData.entryName;
        }
    }

    private void OnEnable()
    {
        if (!discovered)
        {
            undiscoveredShade.SetActive(true);
            journalText.text = "???";
        }
        else
        {
            undiscoveredShade.SetActive(false);
            journalText.text = entryData.entryName;
        }
    }

    public void Discover()
    {
        undiscoveredShade.SetActive(false);
        journalText.text = entryData.entryName;
    }

    public void Click()
    {
        MenuEvent.current.EntrySelect(entryData, discovered);
    }
}
