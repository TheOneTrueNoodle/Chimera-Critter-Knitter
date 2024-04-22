using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Prefabs & Vars")]
    private GameManagement gm;
    public GameData gd;
    [SerializeField] public AreaManager currentAreaManager;

    public GameObject textBoxPrefab; //prefab to instantiate
    public GameObject choicePrefab; //prefab to instantiate
    public GameObject textBoxTarget; //target parent
    public GameObject choiceBoxTarget; //target parent for choices (TEXTBOXTARGET AND CHOICEBOXTARGET ARE SWAPPED ACRTUALLY)
    private GameObject previousLine; //previous textbox
    public bool isInjured = false;

    [Header("UI Variables")]
    [SerializeField] public GameObject contentHolder; //Content
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject spriteHolderLeft;
    [SerializeField] private GameObject spriteHolderRight;
    [SerializeField] private GameObject tailLeft;
    [SerializeField] private GameObject tailRight;
    [SerializeField] public GameObject pawButton;

    [Header("Sprites & Colour")]
    private GameObject spriteHolder;
    private GameObject spriteChildShadow;
    [SerializeField] private ScrollRect dialogueScroll;
    [SerializeField] private GameObject[] dialogueSprites;
    [SerializeField] public GameObject[] choiceSprites;
    private List<GameObject> log = new List<GameObject>();
    public string colourHex;
    [SerializeField] private Color greyOutColor;
    private bool pawPressed = false;
    private bool isOpen = false;

    [Header("Options & Debug")]
    public bool dialogueActive;
    public bool choicesActive;
    public bool showDebuggingText;
    public Conversation currentConvo; //conversation with lines
    public int index = 0; //current line 
    public bool nodialogue = true;

    // Start is called before the first frame update
    void Start()
    {
        Canvas.ForceUpdateCanvases();

        gm = GameObject.Find("GameManagement").GetComponent<GameManagement>();
        gd = GameObject.Find("GameData").GetComponent<GameData>();

        dialogueUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetButtonDown("Interact") || Input.GetButtonDown("Submit") || pawPressed || index == 0) && !nodialogue) //on button press (space) or on 0 lines printed
        {
            pawPressed = false;

            if (index == currentConvo.lines.Length && dialogueActive) //check if we've ran out of lines
            {
                if (currentConvo.choice != null) //if we have run out of lines and there's a choice, show it
                {
                    changeBackgroundColour(dialogueSprites, Color.white);
                    dialogueActive = false;
                    showChoice();
                }
                //else if (currentConvo.fightBegin) //if we have run out of lines and there's a fight
                //{
                    //transitionToBattle(currentConvo.combatName);
                    //exitText(true);
                //}
                else //otherwise if there's no choice and no fight exit dialogue
                {
                    Debug.Log("Exiting Dialog");
                    exitText(true);
                }
            }
            else if (index != currentConvo.lines.Length && dialogueActive) //if we haven't run out of lines, print next line in dialogue
            {
                changeImage(currentConvo.lines[index].speaker, currentConvo.lines[index].emotion); //change image to accomodate speaker
                changeBackgroundColour(dialogueSprites, currentConvo.lines[index].speaker.characterColour);
                //setColours(currentConvo.lines[index].speaker); //change dialogbox border colours according to character
                addText("<uppercase><color=#" + colourHex + ">" + currentConvo.lines[index].speaker.fullName + ":</color></uppercase><br>" + currentConvo.lines[index].text); //(has to be last here)
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
            log.Add(previousLine);
        }

        GameObject newObject = Instantiate(textBoxPrefab); //create
        newObject.transform.SetParent(choiceBoxTarget.transform); //make child of
        newObject.transform.localScale = new Vector3(1, 1, 1); //fix scale problems
        newObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 1); //fix rot problems
        newObject.GetComponent<TextMeshProUGUI>().text = dialogue; //set text in textbox

        previousLine = newObject; //set as previous line
        index++; //move on <<BE CAREFUL OF THIS!!

        openLog(isOpen);

        Canvas.ForceUpdateCanvases();
        dialogueScroll.normalizedPosition = new Vector2(0, 0);//scroll to new text
    }

    public void changeImage(Character character, string emotion) //change the sprite in the UI
    {
        findSide(currentConvo.lines[index].leftSide);
        spriteChildShadow = spriteHolder.transform.parent.gameObject;

        if (currentConvo.onlyOneSpeaker)
        {
            spriteHolderRight.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            spriteHolderRight.transform.parent.gameObject.SetActive(true);
        }

        if (showDebuggingText) { Debug.Log("Changing Image"); }

        if (isInjured && character.fullName == "Oscar")
        {
            switch (emotion)
            {
                case "neutral":
                    spriteHolder.GetComponent<Image>().sprite = character.injuredDefaultPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.injuredDefaultPortrait;
                    break;

                case "default":
                    spriteHolder.GetComponent<Image>().sprite = character.injuredDefaultPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.injuredDefaultPortrait;
                    break;

                case "angry":
                    spriteHolder.GetComponent<Image>().sprite = character.injuredAngryPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.injuredAngryPortrait;
                    break;

                case "happy":
                    spriteHolder.GetComponent<Image>().sprite = character.injuredSmilingPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.injuredSmilingPortrait;
                    break;

                case "sad":
                    spriteHolder.GetComponent<Image>().sprite = character.injuredSadPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.injuredSadPortrait;

                    break;

                case null:
                    spriteHolder.GetComponent<Image>().sprite = character.injuredDefaultPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.injuredDefaultPortrait;
                    break;
            }
        }
        else
        {
            switch (emotion)
            {
                case "neutral":
                    spriteHolder.GetComponent<Image>().sprite = character.defaultPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.defaultPortrait;
                    break;

                case "default":
                    spriteHolder.GetComponent<Image>().sprite = character.defaultPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.defaultPortrait;
                    break;

                case "angry":
                    spriteHolder.GetComponent<Image>().sprite = character.angryPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.angryPortrait;
                    break;

                case "happy":
                    spriteHolder.GetComponent<Image>().sprite = character.smilingPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.smilingPortrait;
                    break;

                case "sad":
                    spriteHolder.GetComponent<Image>().sprite = character.sadPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.sadPortrait;

                    break;

                case null:
                    spriteHolder.GetComponent<Image>().sprite = character.defaultPortrait;
                    spriteChildShadow.GetComponent<Image>().sprite = character.defaultPortrait;
                    break;
            }
        }

    }

    public void findSide(bool isLeft)
    {
        if (isLeft)
        {
            spriteHolder = spriteHolderLeft;
            spriteHolderLeft.GetComponent<Image>().color = Color.white;
            spriteHolderRight.GetComponent<Image>().color = greyOutColor;
            tailLeft.SetActive(true);
            tailRight.SetActive(false);
        }
        else
        {
            spriteHolder = spriteHolderRight;
            spriteHolderLeft.GetComponent<Image>().color = greyOutColor;
            spriteHolderRight.GetComponent<Image>().color = Color.white;
            tailLeft.SetActive(false);
            tailRight.SetActive(true);
        }
    }

    public void nextDialogButton()
    {
        pawPressed = true;
    }

    public void openLog(bool open)
    {
        GameObject last = new GameObject();

        if (log.Count != 0) { last = log.Last(); } //save the last printed line

        nodialogue = open; //disallow continuing dialogue via spacebar

        foreach (GameObject textLogged in log) 
        {
            textLogged.SetActive(open); //reveal or hide all logged dialogue
        }

        if (choicesActive)
        {
            pawButton.SetActive(false);
            nodialogue = true;
            textBoxTarget.SetActive(!open);
            choiceBoxTarget.SetActive(open);
        }
        else
        {
            pawButton.SetActive(!open);
        }
        
    }

    public void changeBackgroundColour(GameObject[] images, Color colour)
    {
        colourHex = ColorUtility.ToHtmlStringRGBA(colour);

        foreach (GameObject image in images)
        {
            image.GetComponent<Image>().color = colour;
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

        log.Clear();
        dialogueActive = false;
        dialogueUI.SetActive(false);
        index = 0;
        DialogueEvents.current.EndDialogue();
    }



    void showChoice()
    {
        pawButton.SetActive(false);
        choicesActive = true;
        //choiceBoxTarget.SetActive(false);

        changeBackgroundColour(choiceSprites, currentConvo.lines[index - 1].speaker.characterColour);

        foreach (GameObject textLogged in log)
        {
            textLogged.SetActive(false);

        }

        if (showDebuggingText) { Debug.Log("Adding Choices"); }

        int iteration = 0;
        foreach (Option option in currentConvo.choice.options)
        {
            
            GameObject newObject = Instantiate(choicePrefab); //create
            newObject.transform.SetParent(textBoxTarget.transform); //make child of
            newObject.transform.localScale = new Vector3(1, 1, 1); //fix scale problems
            newObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 1); //fix rot problems

            //newObject.GetComponent<ChoicePrefabButton>().choice = currentConvo.choice; //old system
            newObject.GetComponentInChildren<ChoicePrefabButton>().choice = currentConvo.choice;
            newObject.GetComponentInChildren<ChoicePrefabButton>().order = iteration;
            iteration++;
        }

        centerChoices();
        //spawn however many buttons with the text from the conversations' Choice scriptable object then set the variable on click

    }

    public void centerChoices()
    {
        RectTransform box = textBoxTarget.GetComponent<RectTransform>();
        Vector2 detle = box.sizeDelta;
        box.anchorMax = new Vector2(0.5f, 0.5f);
        box.anchorMin = new Vector2(0.5f, 0.5f);
        box.pivot = new Vector2(1f, 0.5f);
        box.anchoredPosition = new Vector3((detle[0]/2), (detle[1]/2), 0f);
    }

    public void beginDialogue()
    {
        nodialogue = false;
        dialogueActive = true;
        dialogueUI.SetActive(true);

        DialogueEvents.current.StartDialogue();
    }

    private void transitionToBattle(string combatName)
    {
        //gd.currentGameStatus.nextFight = whatFight;
        //gm.SaveFromSceneToManager();
        //SceneManager.LoadScene(1);

        CombatTrigger[] combats = FindObjectsOfType<CombatTrigger>();
        

        foreach (CombatTrigger combat in combats)
        {
            if (combat.CombatName == combatName)
            {
                combat.Call();
                break;
            }
        }
    }

}
