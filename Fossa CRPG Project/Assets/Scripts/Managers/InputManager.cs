using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Make a function for each input trigger that is connected to all possible functions. 

    public bool InCombat;

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
