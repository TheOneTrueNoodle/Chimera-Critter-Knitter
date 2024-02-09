using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Make a function for each input trigger that is connected to all possible functions. 

    public bool InCombat;
    private bool pauseMenuOpen;
    private bool settingsMenuOpen;
    private bool characterMenuOpen;
    private bool actionSelected;

    private void Update()
    {
        //Default input for space bar
        if (Input.GetButton("Bark") || Input.GetButtonDown("Submit")) 
        {
            BarkOrSelect();
        }  

        //Default input for Escape Key
        if (Input.GetButtonDown("Menu") || Input.GetButtonDown("Cancel"))
        {
            MenuOrCancel();
        }
        
        //Default input for shift key
        if (Input.GetButtonDown("Run"))
        {
            Run();
        }

        //Default input for E key
        if (Input.GetButtonDown("Interact"))
        {
            Interact();
        }
    }

    private void MenuOrCancel()
    {
        switch (InCombat)
        {
            case true:
                //In Combat Input Handler
                if (settingsMenuOpen)
                {

                }
                else if(pauseMenuOpen)
                { 

                }
                else if(characterMenuOpen)
                {

                }
                else if (actionSelected)
                {

                }
                break;
            case false:
                //Out of Combat Input Handler
                break;
        }
    }

    private void BarkOrSelect()
    {

    }

    private void Run()
    {

    }

    private void Interact()
    {

    }
}
