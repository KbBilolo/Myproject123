using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueData dialogueContent;

    public void OpenDialog()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            dialogueManager.StartDialogue(dialogueContent);
        }
    }
}
