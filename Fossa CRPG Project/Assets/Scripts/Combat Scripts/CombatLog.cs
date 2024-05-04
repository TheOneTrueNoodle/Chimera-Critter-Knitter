using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatLog : MonoBehaviour
{
    public Transform logParent;
    public GameObject logPrefab;
    public Scrollbar logScroll;
    public List<GameObject> logs;

    private bool isExpanded = false;
    private Animator anim;

    private void Start()
    {
        CombatEvents.current.onAddLog += NewLog;
        CombatEvents.current.onEndCombat += EndCombat;
        anim = GetComponent<Animator>();

        //Open log at start
        toggleExpand();
    }

    public void NewLog(string Log)
    {
        GameObject newLog = Instantiate(logPrefab, logParent);
        newLog.GetComponent<TMP_Text>().text = Log;
        logScroll.value = 0;
        logs.Add(newLog);
    }

    public void toggleExpand()
    {
        switch(isExpanded)
        {
            case true:
                //Minimize
                anim.Play("Minimize Combat Log");
                logScroll.value = 0;
                isExpanded = false;
                break;
            case false:
                //Expand
                anim.Play("Expand Combat Log");
                logScroll.value = 0;
                isExpanded = true;
                break;
        }
    }

    private void EndCombat(string combatName)
    {
        foreach (GameObject obj in logs)
        {
            Destroy(obj);
        }
        logs.Clear();
    }
}
