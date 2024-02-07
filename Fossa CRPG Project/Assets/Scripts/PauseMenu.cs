using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject SettingsMenuImage;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("EscapePressed");
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void unPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void SettingsMenu()
    {
        SettingsMenuImage.SetActive(true);
    }

    public void SettingsBack()
    {
        SettingsMenuImage.SetActive(false);
    }
}
