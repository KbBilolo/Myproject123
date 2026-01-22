using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueData dialogueData;
    public GameObject talkButton;

    private bool playerInRange;

    void Start()
    {
        if (talkButton != null)
            talkButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (talkButton != null)
                talkButton.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (talkButton != null)
                talkButton.SetActive(false);
        }
    }

    // UI Button calls this
    public void Talk()
    {
        if (!playerInRange) return;

        if (talkButton != null)
            talkButton.SetActive(false);

        dialogueManager.StartDialogue(dialogueData, transform);
    }
}
