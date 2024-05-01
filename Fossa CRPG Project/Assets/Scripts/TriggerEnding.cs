using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEnding : MonoBehaviour
{
    [SerializeField] private Image endingImage;
    [SerializeField] private Sprite goreEnding;
    [SerializeField] private Sprite noGoreEnding;

    [SerializeField] private Animator anim;
    [SerializeField] private int SongID;

    public void Call()
    {
        DialogueEvents.current.StartDialogue();

        switch(PlayerPrefs.GetInt("Remove Gore") == 1)
        {
            case true:
                //Gore is turned off
                endingImage.sprite = noGoreEnding;
                endingImage.preserveAspect = true;
                break;
            default:
                //Gore is turned on
                endingImage.sprite = goreEnding;
                endingImage.preserveAspect = true;
                break;
        }

        AudioManager.instance.SetMusicSong(SongID);

        anim.Play("Ending");
    }
}
