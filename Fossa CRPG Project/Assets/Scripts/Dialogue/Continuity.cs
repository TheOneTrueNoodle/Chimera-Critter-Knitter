using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct Snippet
{
    public Conversation conversation;
    public Choice choice;

}

[System.Serializable]

public struct Thread
{
    public Snippet[] snippets;

}


[CreateAssetMenu(fileName = "New Continuity", menuName = "Dialogue/Continuity")]
public class Continuity : ScriptableObject
{
    [TextArea(2, 5)]
    public string descriptor;
    public Thread[] thread;

}
