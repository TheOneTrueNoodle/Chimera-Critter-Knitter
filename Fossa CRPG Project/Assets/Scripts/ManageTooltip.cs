using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ManageTooltip : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private CanvasScaler scaler;

    // Update is called once per frame
    void Update()
    {
        //tooltip_rect_transform_ref.anchoredPosition = Input.mousePosition / tooltip_parent_canvas.transform.localScale.x;

        Vector2 mergedFactors = new Vector2(
        (canvas.transform as RectTransform).sizeDelta.x / Screen.width,
        (canvas.transform as RectTransform).sizeDelta.y / Screen.height);
        tooltip.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition * mergedFactors;
    }

}
