using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class Character : ScriptableObject
{
    public string fullName; //display name
    public Sprite defaultPortrait; //neutral

    public Sprite smilingPortrait; //happy
    public Sprite sadPortrait; //sad
    public Sprite angryPortrait; //angry

    public Sprite injuredDefaultPortrait; 
    public Sprite injuredSmilingPortrait;
    public Sprite injuredSadPortrait; 
    public Sprite injuredAngryPortrait; 

    public Color characterColour; //text outline clr
}
