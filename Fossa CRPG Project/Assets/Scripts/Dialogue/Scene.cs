using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

[CreateAssetMenu(fileName = "New Scene", menuName = "Scene")]

public class Scene : ScriptableObject
{
    public Conversation[] convos;
    public Choice[] choices;

}
