using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;
    private void Start()
    {
        MenuEvent.current.onOpenMenu += OpenMenu;
        MenuEvent.current.onCloseMenu += CloseMenu;
    }

    private void OpenMenu()
    {
        cam.Priority = 21;
    }
    private void CloseMenu()
    {
        cam.Priority = 8;
    }
}
