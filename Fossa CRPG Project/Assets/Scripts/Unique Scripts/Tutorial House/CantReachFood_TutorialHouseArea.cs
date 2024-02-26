using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantReachFood_TutorialHouseArea : MonoBehaviour
{
    public List<GameObject> mutationZones;

    public void Call()
    {
        AreaManager.current.GetComponent<TutorialHouseArea>().DevelopMutations();
        foreach (GameObject obj in mutationZones)
        {
            obj.SetActive(true);
        }
    }
}
