using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("Choices")]
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;

    [Header("Camera")]
    public GameObject dialogueCamera;
    public GameObject gameplayCamera;

    private DialogueData currentDialogue;
    private int index;

    private List<GameObject> spawnedChoices = new();

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueCamera.SetActive(false);

        nextButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        index = 0;

        gameplayCamera.SetActive(false);
        dialogueCamera.SetActive(true);

        dialoguePanel.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        ClearChoices();

        if (index >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentDialogue.lines[index];

        speakerText.text = line.speaker;
        dialogueText.text = line.text;

        if (line.choices != null && line.choices.Length > 0)
        {
            nextButton.gameObject.SetActive(false);
            SpawnChoices(line.choices);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    void SpawnChoices(DialogueChoice[] choices)
    {
        foreach (var choice in choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            spawnedChoices.Add(btnObj);

            var btn = btnObj.GetComponent<Button>();
            var txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = choice.choiceText;

            btn.onClick.AddListener(() => SelectChoice(choice));
        }
    }

    void ClearChoices()
    {
        foreach (var obj in spawnedChoices)
            Destroy(obj);

        spawnedChoices.Clear();
    }

    void SelectChoice(DialogueChoice choice)
    {
        Debug.Log("Choice clicked: " + choice.choiceText);
        Debug.Log("Next dialogue is: " + choice.nextDialogue);

        ClearChoices();

        if (choice.nextDialogue != null)
        {
            StartDialogue(choice.nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }


    public void NextLine()
    {
        index++;
        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        dialogueCamera.SetActive(false);
        gameplayCamera.SetActive(true);

        currentDialogue = null;
    }
}
