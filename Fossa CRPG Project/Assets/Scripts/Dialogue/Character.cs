using UnityEngine;
using TMPro;

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

    [Header("Character Customisation")]
    public Color characterColour; //text outline clr
    public TMP_FontAsset characterFont;
    public int fontSize;
}
