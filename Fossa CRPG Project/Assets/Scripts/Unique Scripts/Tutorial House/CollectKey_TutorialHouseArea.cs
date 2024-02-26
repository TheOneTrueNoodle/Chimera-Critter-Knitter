using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKey_TutorialHouseArea : MonoBehaviour
{
    public void Call()
    {
        AreaManager.current.GetComponent<TutorialHouseArea>().CollectGateKey();
    }
}
