using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private void Update()
    {
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        bool isOffscreen = pos.x <= 0 || pos.x >= Screen.width ||pos.y <= 0 || pos.y >= Screen.height;

        if (isOffscreen) { Destroy(gameObject); }
    }
}
