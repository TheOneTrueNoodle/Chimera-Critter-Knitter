using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutscene : MonoBehaviour
{

    public GameObject BasementC;
    public GameObject MainCamera;
    public GameObject triggerC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
       if (other.tag == "BasementCutscene")
         {
           
            StartCoroutine(BossCutsceneStart());
        }
    }
    IEnumerator BossCutsceneStart()
    {
        BasementC.SetActive(true);
        MainCamera.SetActive(false);
        triggerC.SetActive(false);
        yield return new WaitForSeconds(18.2f);
        BasementC.SetActive(false);
        MainCamera.SetActive(true);

    }

}
