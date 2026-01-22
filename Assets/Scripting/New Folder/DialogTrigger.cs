using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogData dialogue;
    public DialogueManager dialogueManager;

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
