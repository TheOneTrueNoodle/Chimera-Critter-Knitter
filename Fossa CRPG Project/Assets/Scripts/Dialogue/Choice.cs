using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct Option
{
    [TextArea(2, 5)]
    public string choiceText;
    public bool startFight;
    public Conversation continueDialogue;
    public string progressionID;
}

[CreateAssetMenu(fileName = "New Choice", menuName = "Dialogue/Choice")]
public class Choice : ScriptableObject
{
    public Option[] options;
    
}
