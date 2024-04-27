using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCombatcutscene : MonoBehaviour
{
    public GameObject FirstCombatCutscene;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player is here");
            StartCoroutine(FirstCombatCutscene1());
        }
    }

    IEnumerator FirstCombatCutscene1()
    {
        DialogueEvents.current.StartDialogue();
        FirstCombatCutscene.SetActive(true);

        yield return new WaitForSeconds(8.5f);

        FirstCombatCutscene.SetActive(false);
        DialogueEvents.current.EndDialogue();

        GetComponent<CombatTrigger>().Call();
        Destroy(gameObject);
    }
}
