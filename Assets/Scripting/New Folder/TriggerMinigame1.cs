using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMinigame1 : MonoBehaviour
{
    private CameraManager cameraManager;
    public GameObject startpanel;
    public GameObject MiniGamepanel;

    private bool playerInRange = false;

    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        startpanel.SetActive(false);
        //MiniGamepanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startpanel.SetActive(true);
            playerInRange = true;
            Debug.Log("Player in range, panel shown");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startpanel.SetActive(false);
            playerInRange = false;
            Debug.Log("Player left range, panel hidden");
        }
    }

    // Assign this method to the button's OnClick event in the Inspector
    public void OnStartMinigameButtonPressed()
    {
        if (playerInRange && cameraManager != null)
        {
            cameraManager.ChangeMinigame();
            startpanel.SetActive(false);
            MiniGamepanel.SetActive(true);
            Debug.Log("Minigame started");
        }
    }
}
