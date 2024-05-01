using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public string tooltipText;
    [SerializeField] private int waitTime = 1;
    [SerializeField] private ManageTooltip tpmanager;
    public string side;
    private IEnumerator coroutine;

    void Update()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            StopCoroutine(coroutine);
            tpmanager.HideTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        coroutine = WaitAndReveal(waitTime);
        StartCoroutine(coroutine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(coroutine);
        tpmanager.HideTooltip();
    }

    IEnumerator WaitAndReveal(int time)
    {
        yield return new WaitForSeconds(time);
        print(tooltipText);
        tpmanager.ShowTooltip(tooltipText, side, this.gameObject);
    }
}
