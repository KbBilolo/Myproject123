using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StoryDialogManager : MonoBehaviour
{
    
    public Button nextButton;

    // Added missing fields
    public GameObject dialoguePanel; // For dialoguePanel
    public List<Button> choiceButtons; // For choiceButtons
    private StoryDialogData currentDialogue; // For currentDialogue
    private int index; // For index
    public TextMeshProUGUI speakerText; // For speakerText
    public TextMeshProUGUI dialogueText; // For dialogueText

    void Start()
    {
        dialoguePanel.SetActive(false);

        // hide choices for story mode
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);

        nextButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(StoryDialogData dialogue)
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

        // Fire any events for this line
        line.InvokeEvents();

        // Handle choices
        if (line.choices != null && line.choices.Count > 0)
        {
            nextButton.gameObject.SetActive(false);

            for (int i = 0; i < choiceButtons.Count; i++)
            {
                if (i < line.choices.Count)
                {
                    choiceButtons[i].gameObject.SetActive(true);
                    var choice = line.choices[i];
                    choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

                    // Remove previous listeners to avoid stacking
                    choiceButtons[i].onClick.RemoveAllListeners();

                    // Capture index for closure
                    int choiceIndex = i;
                    choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // No choices, just show next button
            foreach (var btn in choiceButtons)
                btn.gameObject.SetActive(false);

            nextButton.gameObject.SetActive(true);
        }
    }

    public void NextLine()
    {
        index++;

        if (index >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void OnChoiceSelected(int choiceIndex)
    {
        var line = currentDialogue.lines[index];
        var choice = line.choices[choiceIndex];

        // Fire choice event if any
        choice.onChoiceSelected?.Invoke();

        // Go to the next line as specified by the choice
        if (choice.nextLineIndex >= 0 && choice.nextLineIndex < currentDialogue.lines.Count)
        {
            index = choice.nextLineIndex;
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;

        // Hide all choice buttons when dialog ends
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);
    }


}

