using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct Line
{
    public Character speaker;
    public string emotion;

    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    public Line[] lines;
    public Choice choice;
    public bool fightBegin;
    public Battle battle;
}
