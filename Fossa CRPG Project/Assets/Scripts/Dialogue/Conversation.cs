using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct Line
{
    public Character speaker;
    public string emotion;
    public bool leftSide;


    [TextArea(2, 5)]
    public string text;
    //public TextEffect effect;
}

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject
{
    public Line[] lines;
    public Choice choice;
    public bool fightBegin;
    public string combatName;
    public bool onlyOneSpeaker;

}
/*
public enum TextEffect
{
    None,
    Italics,
    Bold,
    Underline
}
*/
