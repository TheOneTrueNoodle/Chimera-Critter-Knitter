using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueZoneTrigger : MonoBehaviour
{
    [SerializeField] private bool oneTimeConversation;
    public bool withInteractionTriggerScript;
    //[SerializeField] private bool needsInteraction;
    [SerializeField] private Conversation convoStart;
    public DialogueManager dm;
    //public bool spawnSpeaker;

    void Start()
    {
        dm = GameObject.Find("DialogueManagement").GetComponent<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !withInteractionTriggerScript)
        {
            dm.currentConvo = convoStart;

            dm.beginDialogue();

            /*if (spawnSpeaker)
            {
                //GameObject.Find("Speaker_A").SetActive(true);
            }*/

            if (oneTimeConversation)
            {
                Destroy(this);
            }
        }
    }

    public void DialogueButtonTrigger()
    {
        dm.currentConvo = convoStart;
        dm.beginDialogue();
        //Debug.Log("DIALOGBUTTON");
    }
}
