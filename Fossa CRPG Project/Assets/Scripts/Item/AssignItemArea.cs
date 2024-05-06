using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignItemArea : MonoBehaviour
{
    [SerializeField] private Transform parent;
    public void AssignArea()
    {
        FindObjectOfType<HeldItem>().currentAreaParent = parent;
    }
}
