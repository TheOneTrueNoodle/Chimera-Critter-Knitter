using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExamineObject : MonoBehaviour, IDragHandler
{
    public Transform objectSpawnLocation;

    private GameObject dataPrefab;

    public void OnEntrySelect(JournalEntryData Data)
    {
        if (dataPrefab != null)
        {
            Destroy(dataPrefab.gameObject);
        }
        dataPrefab = Instantiate(Data.prefab, objectSpawnLocation.position, Quaternion.identity, objectSpawnLocation);
        dataPrefab.layer = dataPrefab.transform.parent.gameObject.layer;

        var children = dataPrefab.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = dataPrefab.transform.parent.gameObject.layer;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dataPrefab.transform.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }
}
