using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExamineObject : MonoBehaviour, IDragHandler
{
    public Transform objectSpawnLocation;

    private GameObject dataPrefab;

    [SerializeField] private Camera inspectCam;
    [Range(0.5f, 7f)] public float zoom = 4f;

    private void Start()
    {
        MenuEvent.current.onEntrySelect += OnEntrySelect;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoom -= Input.GetAxis("Mouse ScrollWheel");
            zoom = Mathf.Clamp(zoom, 0.5f, 7f);
            inspectCam.orthographicSize = zoom;
        }
    }

    public void OnEntrySelect(JournalEntryData Data, bool discovered)
    {
        if (dataPrefab != null)
        {
            Destroy(dataPrefab.gameObject);
        }
        if (!discovered) { return; }

        dataPrefab = Instantiate(Data.prefab, objectSpawnLocation.position, Quaternion.identity, objectSpawnLocation);
        dataPrefab.layer = dataPrefab.transform.parent.gameObject.layer;

        var children = dataPrefab.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = dataPrefab.transform.parent.gameObject.layer;
        }
    }

    public void ZoomIn()
    {
        zoom -= 0.1f;
        zoom = Mathf.Clamp(zoom, 0.5f, 7f);
        inspectCam.orthographicSize = zoom;
    }

    public void ZoomOut()
    {
        zoom += 0.1f;
        zoom = Mathf.Clamp(zoom, 0.5f, 7f);
        inspectCam.orthographicSize = zoom;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dataPrefab.transform.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }
}
