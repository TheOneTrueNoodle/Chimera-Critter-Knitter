using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCombatCutscene : MonoBehaviour
{
    public GameObject SecondCombatCutscene2;

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
          
            StartCoroutine(SecondCombatCutscene1());
        }
    }

    IEnumerator SecondCombatCutscene1()
    {
        SecondCombatCutscene2.SetActive(true);
        yield return new WaitForSeconds(3f);
        SecondCombatCutscene2.SetActive(false);
        Destroy(gameObject);

    }
}
