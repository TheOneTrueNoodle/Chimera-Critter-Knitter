using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class HoverTipManager : MonoBehaviour
{
    public TMP_Text tipText;
    public RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void Start()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.gameObject.SetActive(true);
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 300 ? 300 : tipText.preferredWidth, tipText.preferredHeight + 5);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x, mousePos.y);
    }

    public void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
}
