using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChoicePrefabButton : MonoBehaviour
{
    public Button button;
    public DialogueManager dm;
    public Choice choice;
    private ScrollRect dialogueScroll;
    [SerializeField] private Character choiceMaker;


    void Start()
    {
        AddEventScript(this.gameObject);
        button = GetComponent<Button>();
        button.onClick.AddListener(taskOnClick);
        dm = GameObject.Find("DialogueManagement").GetComponent<DialogueManager>();
        dialogueScroll = GameObject.Find("DialogueHolder").GetComponent<ScrollRect>();

        initialiseText(); //set button text
    }

    private void AddEventScript(GameObject gm)
    {
        if (gm.GetComponent<EventTrigger>() == null)
        {
            gm.AddComponent<EventTrigger>();
        }
        EventTrigger trigger = gm.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entryTwo = new EventTrigger.Entry();


        entry.eventID = EventTriggerType.PointerEnter;
        entryTwo.eventID = EventTriggerType.PointerExit;

        entry.callback.AddListener((functionIWant) => { fontUp(); });
        entryTwo.callback.AddListener((functionIWant) => { fontDown(); });

        trigger.triggers.Add(entry);
        trigger.triggers.Add(entryTwo);

    }

    public void fontUp()
    {
        //this.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().fontSize = 20; old version
        GetComponent<TextMeshProUGUI>().fontSize = 20;
    }

    public void fontDown()
    {
        //this.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().fontSize = 22; old version
        GetComponent<TextMeshProUGUI>().fontSize = 22;
    }

    public void taskOnClick()
    {
        Debug.Log("BUTTONPRESSED");

        int j = 0;
        GameObject content = GameObject.Find("Content");
        foreach (Transform child in content.transform)
        {
            if (child.tag == "Dialogue")
            {
                j++;
            }
        }
        int i = transform.GetSiblingIndex();


        dm.currentConvo = this.choice.options[(i - j)].continueDialogue;
        dm.dialogueActive = true;

        dm.addText(choiceMaker.fullName + ":<br>" + this.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text); //add dialogue
        dm.index = 0;

        var objects = GameObject.FindGameObjectsWithTag("Choice"); //destory buttons
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    private void initialiseText()
    {
        int x = 0;
        GameObject content = GameObject.Find("Content");
        foreach (Transform child in content.transform)
        {
            if (child.tag == "Dialogue")
            {
                x++;
            }
        }

        int y = transform.GetSiblingIndex();
        //Debug.Log(y);
        //Debug.Log(x);

        GetComponent<TextMeshProUGUI>().text = choice.options[(y - x)].choiceText; //set text in textbox

        //this.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text = choice.options[(y - x)].choiceText; //show correct text (this is for old version)
        //dm.changeImage(choiceMaker, "neutral"); //change the sprite in the UI


        Canvas.ForceUpdateCanvases();
        dialogueScroll.normalizedPosition = new Vector2(0, 0); //scroll to new text
    }
}
