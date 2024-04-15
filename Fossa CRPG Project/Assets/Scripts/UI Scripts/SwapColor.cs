using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapColor : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image image;

    public Color normalColor;
    public Color newColor;

    public void Swap()
    {
        image.color = newColor;
    }

    public void Revert()
    {
        image.color = normalColor;
    }
}
