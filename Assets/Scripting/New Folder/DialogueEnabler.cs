using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueData dialogueData;
    public GameObject talkButton; // mobile UI button

    private bool playerInRange;

    void Start()
    {
        talkButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            talkButton.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            talkButton.SetActive(false);
        }
    }

    // Called by UI Button
    public void Talk()
    {
        if (!playerInRange) return;

        talkButton.SetActive(false);
        dialogueManager.StartDialogue(dialogueData);
    }

}
