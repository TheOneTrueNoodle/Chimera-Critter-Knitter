using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPText : MonoBehaviour
{
    public TMP_Text xpText;
    public void Setup(int xp)
    {
        xpText.text = "+" + xp.ToString();
    }
}
