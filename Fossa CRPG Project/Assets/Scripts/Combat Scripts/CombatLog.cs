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

    private void Start()
    {
        CombatEvents.current.onAddLog += NewLog;
    }

    public void NewLog(string Log)
    {
        GameObject newLog = Instantiate(logPrefab, logParent);
        newLog.GetComponent<TMP_Text>().text = Log;
        logScroll.value = 1;
    }
}
