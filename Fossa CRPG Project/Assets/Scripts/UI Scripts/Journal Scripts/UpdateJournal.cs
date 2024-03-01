using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateJournal : MonoBehaviour
{
    public JournalEntryData entryData;

    public void Call()
    {
        MenuEvent.current.FindEntryObject(entryData);
    }
}
