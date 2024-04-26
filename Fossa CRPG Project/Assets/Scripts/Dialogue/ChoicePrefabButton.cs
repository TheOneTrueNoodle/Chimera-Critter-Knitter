using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChoicePrefabButton : MonoBehaviour
{
    public Button button;
    public int order = 0;
    public DialogueManager dm;
    public Choice choice;
    private ScrollRect dialogueScroll;
    [SerializeField] private Character choiceMaker;
    public TextMeshProUGUI textChoiceField;
    public AreaManager relevantAreaManager;


    void Start()
    {
       // AddEventScript(this.gameObject);
        button = GetComponent<Button>();
        button.onClick.AddListener(taskOnClick);
        dm = GameObject.Find("DialogueManagement").GetComponent<DialogueManager>();
        dialogueScroll = GameObject.Find("DialogueHolder").GetComponent<ScrollRect>();
        initialiseText(); //set button text
        textChoiceField = this.GetComponentInChildren<TextMeshProUGUI>();
        relevantAreaManager = dm.currentAreaManager;
    }

    /*
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

    }*/


    public void taskOnClick()
    {
        dm.choiceBoxTarget.SetActive(true);

        /*
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
        */
        checkForFlagChanges(this.choice.options[order].progressionID);

        dm.changeBackgroundColour(dm.choiceSprites, Color.white);
        dm.pawButton.SetActive(true);
        dm.choicesActive = false;

        if (this.choice.options[order].continueDialogue != null)
        {
            dm.currentConvo = this.choice.options[order].continueDialogue;
            dm.dialogueActive = true;
            dm.nodialogue = false;

            string cler = ColorUtility.ToHtmlStringRGB(choiceMaker.characterColour);

            //dm.addText(choiceMaker.fullName + ":<br>" + this.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text); //old add dialogue
            dm.addText("<uppercase><color=#" + cler + ">" + choiceMaker.fullName + ":</color></uppercase><br>" + textChoiceField.text, choiceMaker, false); //add dialogue
            dm.index = 0;

            var objects = GameObject.FindGameObjectsWithTag("Choice"); //destory buttons
            foreach (var obj in objects)
            {
                Destroy(obj);
            }
        }
        else
        {

            dm.exitText(true);
        }
        
    }

    private void checkForFlagChanges(string ID)
    {
        //Debug.Log(relevantAreaManager.areaBools.ContainsKey(ID));

        if (relevantAreaManager.areaBools.ContainsKey(ID))
        {
            relevantAreaManager.areaBools[ID] = true;
            Debug.Log("Area Manager Bool for " + textChoiceField.text + "is now " + relevantAreaManager.areaBools[ID]);

        }
    }

    private void initialiseText()
    {
        /*int x = 0;
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
        */
        Debug.Log(order);
        textChoiceField.text = choice.options[order].choiceText; //set text in textbox
        //GetComponentInParent<TextMeshProUGUI>().text = choice.options[(y - x)].choiceText; //set text in textbox

        //this.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text = choice.options[(y - x)].choiceText; //show correct text (this is for old version)
        //dm.changeImage(choiceMaker, "neutral"); //change the sprite in the UI


        Canvas.ForceUpdateCanvases();
        dialogueScroll.normalizedPosition = new Vector2(0, 0); //scroll to new text
    }
}
