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
    public GameObject playercontrolUi;

    [Header("Typewriter")]
    public float typingSpeed = 0.03f;

    private DialogueData currentDialogue;
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
        // Tap anywhere (mobile / PC)
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0) && nextButton.gameObject.activeSelf)
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData dialogue, Transform npc = null)
    {
        currentDialogue = dialogue;
        index = 0;

        if (playerMovement != null)
            playerMovement.enabled = false;
        if (playercontrolUi != null)
            playercontrolUi.SetActive(false);

        if (npc != null && Camera.main != null)
        {
            Vector3 dir = npc.position - Camera.main.transform.position;
            dir.y = 0;
            Camera.main.transform.rotation = Quaternion.LookRotation(dir);
        }

        gameplayCamera.SetActive(false);
        dialogueCamera.SetActive(true);

        nextButton.gameObject.SetActive(true);
        dialoguePanel.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        ClearChoices();
        nextButton.gameObject.SetActive(true);

        if (index >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[index];

        speakerText.text = line.speaker;
        StartTyping(line.text);

        if (line.choices != null && line.choices.Length > 0)
        {
            nextButton.gameObject.SetActive(false);
            SpawnChoices(line.choices);
        }
        if (line.startQuest != null)
        {
            QuestManager.Instance.StartQuest(line.startQuest);
        }

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

        if (choice.nextDialogue != null)
            StartDialogue(choice.nextDialogue);
        if (choice.startQuest != null)
        {
            QuestManager.Instance.StartQuest(choice.startQuest);
        }
        else
            EndDialogue();
        

    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingRoutine);
            dialogueText.text = currentDialogue.lines[index].text;
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
        if (playercontrolUi != null)
            playercontrolUi.SetActive(true);

        currentDialogue = null;
    }
}
