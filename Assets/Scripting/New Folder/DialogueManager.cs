using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
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

    [Header("Player")]
    public MonoBehaviour playerMovement;
    public GameObject playerControlUI;

    [Header("Typewriter")]
    public float typingSpeed = 0.03f;

    private DialogueLine[] currentLines;
    private int index;

    private List<GameObject> spawnedChoices = new();
    private Coroutine typingRoutine;
    private bool isTyping;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueCamera.SetActive(false);

        nextButton.onClick.AddListener(NextLine);
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0) && nextButton.gameObject.activeSelf)
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData dialogue, Transform npc = null)
    {
        currentLines = ResolveDialogue(dialogue);

        if (currentLines == null || currentLines.Length == 0)
        {
            Debug.LogWarning("No valid dialogue found");
            return;
        }

        index = 0;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerControlUI != null)
            playerControlUI.SetActive(false);

        gameplayCamera.SetActive(false);
        dialogueCamera.SetActive(true);

        dialoguePanel.SetActive(true);
        nextButton.gameObject.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        ClearChoices();

        if (index >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentLines[index];

        speakerText.text = line.speaker;
        StartTyping(line.text);

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

    DialogueLine[] ResolveDialogue(DialogueData dialogue)
    {
        foreach (var condition in dialogue.conditions)
        {
            bool match = condition.requiredState switch
            {
                QuestState.NotStarted =>
                    !QuestManager.Instance.IsQuestActive(condition.quest),

                QuestState.InProgress =>
                    QuestManager.Instance.IsQuestActive(condition.quest) &&
                    !QuestManager.Instance.IsQuestCompleted(condition.quest),

                QuestState.Completed =>
                    QuestManager.Instance.IsQuestCompleted(condition.quest),

                _ => false
            };

            if (match)
                return condition.lines;
        }

        return null;
    }

    void StartTyping(string text)
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void SpawnChoices(DialogueChoice[] choices)
    {
        foreach (DialogueChoice choice in choices)
        {
            DialogueChoice localChoice = choice;

            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            spawnedChoices.Add(btnObj);

            Button btn = btnObj.GetComponent<Button>();
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = localChoice.choiceText;
            btn.onClick.AddListener(() => SelectChoice(localChoice));
        }
    }

    void ClearChoices()
    {
        foreach (GameObject obj in spawnedChoices)
            Destroy(obj);

        spawnedChoices.Clear();
    }

    void SelectChoice(DialogueChoice choice)
    {
        ClearChoices();

        if (choice.startQuest != null)
            QuestManager.Instance.StartQuest(choice.startQuest);

        if (choice.nextDialogue != null)
            StartDialogue(choice.nextDialogue);
        else
            EndDialogue();
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingRoutine);
            dialogueText.text = currentLines[index].text;
            isTyping = false;
            return;
        }

        index++;
        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueCamera.SetActive(false);
        gameplayCamera.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerControlUI != null)
            playerControlUI.SetActive(true);
    }
}
