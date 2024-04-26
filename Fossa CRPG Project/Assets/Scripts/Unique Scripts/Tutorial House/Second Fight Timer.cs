using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFightTimer : MonoBehaviour
{
    [SerializeField] private float timerLength = 120f;

    private bool playerInRoom;

    public float currentTimer;

    [SerializeField] private GameObject roomBeforeTimer;
    [SerializeField] private GameObject roomAfterTimer;
    [field: SerializeField] public FMODUnity.EventReference glassBreakSFX { get; private set; }
    [SerializeField] private Transform glassBreakSource;


    private void Update()
    {
        currentTimer += Time.deltaTime;

        if (playerInRoom)
        {
            if (timerLength - currentTimer <= 5f)
            {
                //Within 5 seconds of timer ending
                currentTimer = timerLength - 5f;
            }
        }

        if (currentTimer >= timerLength)
        {
            //Timer is up!!!!
            roomBeforeTimer.SetActive(false);
            roomAfterTimer.SetActive(true);
            AudioManager.instance.PlayOneShot(glassBreakSFX, glassBreakSource.position);
            MenuEvent.current.SpawnPopup("Something broke into the study...");

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement pm))
        {
            playerInRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement pm))
        {
            playerInRoom = false;
        }
    }

    public void TriggerTimer()
    {
        currentTimer = timerLength;
    }

    private void OnEnable()
    {
        currentTimer = 0;
    }
}
