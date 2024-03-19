using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct Snippet
{
    public Conversation conversation;

    [TextArea(2, 5)]
    public string[] choiceID;

}

[System.Serializable]

public struct Thread
{
    public Snippet[] snippets;

}


[CreateAssetMenu(fileName = "New Continuity", menuName = "Dialogue/Continuity")]
public class Continuity : ScriptableObject
{
    public Thread[] thread;

}
