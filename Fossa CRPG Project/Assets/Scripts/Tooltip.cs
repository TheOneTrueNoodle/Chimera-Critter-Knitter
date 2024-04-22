using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public string tooltipText;
    [SerializeField] private int waitTime = 1;
    private IEnumerator coroutine;


    public void OnPointerEnter(PointerEventData eventData)
    {
        coroutine = WaitAndReveal(waitTime);
        StartCoroutine(coroutine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(coroutine);
    }

    IEnumerator WaitAndReveal(int time)
    {
        yield return new WaitForSeconds(time);
        print(tooltipText);
    }
}
