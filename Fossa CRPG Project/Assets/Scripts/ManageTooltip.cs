using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ManageTooltip : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tooltipR;
    [SerializeField] private GameObject tooltipR2;
    [SerializeField] private GameObject tooltipL;
    [SerializeField] private GameObject tooltipL2;
    [SerializeField] private CanvasScaler scaler;
    private GameObject lastTltp;

    // Update is called once per frame
    void Update()
    {
        //tooltip_rect_transform_ref.anchoredPosition = Input.mousePosition / tooltip_parent_canvas.transform.localScale.x;

        Vector2 mergedFactors = new Vector2(
        (canvas.transform as RectTransform).sizeDelta.x / Screen.width,
        (canvas.transform as RectTransform).sizeDelta.y / Screen.height);
        tooltipR.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition * mergedFactors;
        tooltipL.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition * mergedFactors;

        if (lastTltp != null && !lastTltp.activeInHierarchy)
        {
            HideTooltip();
            lastTltp = null;
        }
    }

    public void ShowTooltip(string txt, string side, GameObject obj)
    {
        lastTltp = obj;

        switch (side)
        {
            case "left":
                tooltipL.GetComponent<TextMeshProUGUI>().text = txt;
                tooltipL2.GetComponent<TextMeshProUGUI>().text = txt;
                tooltipL.SetActive(true);
                break;

            case "right":
                tooltipR.GetComponent<TextMeshProUGUI>().text = txt;
                tooltipR2.GetComponent<TextMeshProUGUI>().text = txt;
                tooltipR.SetActive(true);
                break;

            case null:
                tooltipR.SetActive(true);
                break;
        }

    }

    public void HideTooltip()
    {
        tooltipR.SetActive(false);
        tooltipL.SetActive(false);
    }

}
