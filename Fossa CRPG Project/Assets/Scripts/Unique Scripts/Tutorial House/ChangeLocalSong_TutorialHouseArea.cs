using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocalSong_TutorialHouseArea : MonoBehaviour
{
    [SerializeField] private int newSongParameter;

    public void Call()
    {
        if (AreaManager.current.areaBools["InCombat"] == true) { return; }
        if (AreaManager.current.areaBools.ContainsKey("defeatedOwner") && AreaManager.current.areaBools["defeatedOwner"] != true)
        {
            //Chagne Music
            Debug.Log(AreaManager.current.areaBools["InCombat"]);

            AudioManager.instance.SetMusicSong(newSongParameter);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Call();
        }
    }
}
