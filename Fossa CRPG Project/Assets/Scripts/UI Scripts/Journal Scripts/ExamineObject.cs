using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExamineObject : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform objectSpawnLocation;

    private GameObject dataPrefab;

    [SerializeField] private Camera inspectCam;
    [Range(0.5f, 7f)] public float zoom = 4f;

    private bool mouseOver;

    private Vector3 prevMousePos = Vector3.zero;
    private Vector3 posDelta = Vector3.zero;

    private void Start()
    {
        MenuEvent.current.onEntrySelect += OnEntrySelect;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && mouseOver)
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

        dataPrefab = Instantiate(Data.prefab, objectSpawnLocation.position, Data.prefab.transform.localRotation, objectSpawnLocation);
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
        //Vector3 newRotation = new Vector3(dataPrefab.transform.localRotation.x + eventData.delta.y, dataPrefab.transform.localRotation.y - eventData.delta.x, dataPrefab.transform.localRotation.z);
        //dataPrefab.transform.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
        //dataPrefab.transform.localRotation = Quaternion.Euler(newRotation);

        float rotationVelocityX = eventData.delta.x * 1;
        float rotationVelocityY = eventData.delta.y * 1;
        dataPrefab.transform.Rotate(Vector3.right, -rotationVelocityY, Space.Self);
        dataPrefab.transform.Rotate(Vector3.up, -rotationVelocityX, Space.Self);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }
}
