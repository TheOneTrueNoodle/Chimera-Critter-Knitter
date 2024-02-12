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
        FirstCombatCutscene.SetActive(true);
        yield return new WaitForSeconds(3f);
        FirstCombatCutscene.SetActive(false);
        Destroy(gameObject);

    }
}