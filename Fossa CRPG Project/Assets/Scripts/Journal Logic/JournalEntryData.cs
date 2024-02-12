using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Journal Entry", menuName = "Journal/Entry", order = 3)]
public class JournalEntryData : ScriptableObject
{
    public Sprite UIImage;
    public GameObject prefab;

}
