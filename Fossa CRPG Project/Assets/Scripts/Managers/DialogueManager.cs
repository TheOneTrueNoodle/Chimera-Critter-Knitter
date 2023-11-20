using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private GameManagement gm;
    public GameData gd;

    public GameObject textBoxPrefab; //prefab to instantiate
    public GameObject choicePrefab; //prefab to instantiate
    public GameObject textBoxTarget; //target parent
    public GameObject previousLine; //previous textbox

    private GameObject contentHolder; //Content
    private GameObject dialogueUI;
    private GameObject spriteHolder;
    private ScrollRect dialogueScroll;

    [Header("OPTIONS")]
    public bool dialogueActive;
    public bool showDebuggingText;
    public Conversation currentConvo; //conversation with lines
    public int index = 0; //current line 
    public bool nodialogue = true;

    // Start is called before the first frame update
    void Start()
    {
        Canvas.ForceUpdateCanvases();

        dialogueUI = GameObject.Find("Dialogue");
        contentHolder = GameObject.Find("Content");
        spriteHolder = GameObject.Find("SpeakerImage");
        dialogueScroll = GameObject.Find("DialogueHolder").GetComponent<ScrollRect>();

        gm = GameObject.Find("GameManagement").GetComponent<GameManagement>();
        gd = GameObject.Find("GameData").GetComponent<GameData>();


        dialogueUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetButtonDown("Interact") || Input.GetButtonDown("Submit") || index == 0) && !nodialogue) //on button press (space) or on 0 lines printed
        {
            if (index == currentConvo.lines.Length && dialogueActive) //check if we've ran out of lines
            {
                if (currentConvo.choice != null) //if we have run out of lines and there's a choice, show it
                {
                    dialogueActive = false;
                    showChoice();
                }
                else if (currentConvo.fightBegin) //if we have run out of lines and there's a fight
                {
                    transitionToBattle();
                    exitText(true);
                }
                else //otherwise if there's no choice and no fight exit dialogue
                {
                    Debug.Log("Exiting Dialog");
                    exitText(true);
                }
            }
            else if (index != currentConvo.lines.Length && dialogueActive) //if we haven't run out of lines, print next line in dialogue
            {
                changeImage(currentConvo.lines[index].speaker, currentConvo.lines[index].emotion); //change image to accomodate speaker
                addText(currentConvo.lines[index].speaker.fullName + ":<br>" + currentConvo.lines[index].text); //(has to be last here)
            }
        }

    }

    public void addText(string dialogue) //instantiate text game object with dialogue as text childed under textBoxTarget
    {
        if (showDebuggingText) { Debug.Log("Adding Text"); }

        if (previousLine != null)
        {
            string temp = previousLine.GetComponent<TextMeshProUGUI>().text;
            previousLine.GetComponent<TextMeshProUGUI>().text = "<color=grey>" + temp;
        }

        GameObject newObject = Instantiate(textBoxPrefab); //create
        newObject.transform.SetParent(textBoxTarget.transform); //make child of
        newObject.transform.localScale = new Vector3(1, 1, 1); //fix scale problems
        newObject.GetComponent<TextMeshProUGUI>().text = dialogue; //set text in textbox

        previousLine = newObject; //set as previous line
        index++; //move on <<BE CAREFUL OF THIS!!

        Canvas.ForceUpdateCanvases();
        dialogueScroll.normalizedPosition = new Vector2(0, 0);//scroll to new text
    }

    public void changeImage(Character character, string emotion) //change the sprite in the UI
    {
        if (showDebuggingText) { Debug.Log("Changing Image"); }

        switch (emotion)
        {
            case "neutral":
                spriteHolder.GetComponent<Image>().sprite = character.defaultPortrait;
                break;

            case "angry":
                spriteHolder.GetComponent<Image>().sprite = character.angryPortrait;
                break;

            case "happy":
                spriteHolder.GetComponent<Image>().sprite = character.smilingPortrait;
                break;

            case "sad":
                spriteHolder.GetComponent<Image>().sprite = character.sadPortrait;
                break;

            case null:
                spriteHolder.GetComponent<Image>().sprite = character.defaultPortrait;
                break;
        }
    }

    public void exitText(bool destroyold) //hide ui and reset vars for next time
    {
        if (showDebuggingText) { Debug.Log("Exiting Dialogue"); }

        if (destroyold)
        {
            foreach (Transform child in contentHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        dialogueActive = false;
        dialogueUI.SetActive(false);
        index = 0;
        DialogueEvents.current.EndDialogue();

    }

    void showChoice()
    {
        if (showDebuggingText) { Debug.Log("Adding Choices"); }

        foreach (Option option in currentConvo.choice.options)
        {

            GameObject newObject = Instantiate(choicePrefab); //create
            newObject.transform.SetParent(textBoxTarget.transform); //make child of
            newObject.transform.localScale = new Vector3(1, 1, 1); //fix scale problems
            newObject.GetComponent<ChoicePrefabButton>().choice = currentConvo.choice;

        }

        //spawn two buttons with the text from the conversations' Choice scriptable object then set the variable on click
    }

    public void beginDialogue()
    {
        nodialogue = false;
        dialogueActive = true;
        dialogueUI.SetActive(true);

        DialogueEvents.current.StartDialogue();
    }

    private void transitionToBattle()
    {
        //gd.currentGameStatus.nextFight = whatFight;
        //gm.SaveFromSceneToManager();
        //SceneManager.LoadScene(1);

        Debug.Log("Beginning Combat");
        //CombatEvents.current.StartCombat();
    }

}
