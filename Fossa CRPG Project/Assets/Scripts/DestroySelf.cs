using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    void destroyMe()
    {
        Destroy(this.gameObject);
        
    }

}
