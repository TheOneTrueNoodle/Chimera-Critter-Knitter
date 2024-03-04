using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera localCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            localCamera.Priority = 15;
            StartCoroutine(other.GetComponent<PlayerMovement>().resetCameraForward());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            localCamera.Priority = 10;
        }
    }
}
