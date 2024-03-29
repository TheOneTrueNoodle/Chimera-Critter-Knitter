using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField] private TMP_Text popupText;

    public void Setup(string text)
    {
        popupText.text = text;
    }
}
