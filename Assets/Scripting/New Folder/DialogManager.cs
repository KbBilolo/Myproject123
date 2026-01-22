using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Button nextButton;
    public Button[] choiceButtons;

    private DialogData currentDialogue;
    private int index;

    void Start()
    {
        dialoguePanel.SetActive(false);

        // hide choices for story mode
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);

        nextButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(DialogData dialogue)
    {
        currentDialogue = dialogue;
        index = 0;

        dialoguePanel.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        var line = currentDialogue.lines[index];
        speakerText.text = line.speaker;
        dialogueText.text = line.text;
    }

    void NextLine()
    {
        index++;

        if (index >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }
    
}
