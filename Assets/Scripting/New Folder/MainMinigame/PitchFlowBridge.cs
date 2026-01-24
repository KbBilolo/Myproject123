using UnityEngine;

public class PitchFlowBridge : MonoBehaviour
{
    [Header("Refs")]
    public IdeationPitchMinigameManager minigameManager;
    public IdeationPitchUIController_Slider minigameUI; // your UI controller
    public Canvas minigameCanvas;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;
    public DialogueData profAfterRoundDialogue;
    public DialogueData profOnSuccessDialogue;
    public DialogueData profOnFailDialogue;

    [Header("Quest Tracking (optional but recommended)")]
    public QuestData pitchRoundsQuest;   // requiredAmount = 4 (tracks round count)
    public QuestData pitchSuccessQuest;  // requiredAmount = 1
    public QuestData pitchFailQuest;     // requiredAmount = 1

    void Start()
    {
        // Initially hide the minigame UI
        if (minigameCanvas != null) minigameCanvas.enabled = false;
    }

    public void ReturnToProfessor()
    {
        // Hide the minigame UI
        if (minigameCanvas != null) minigameCanvas.enabled = false;

        // Track that a round finished
        if (pitchRoundsQuest != null)
        {
            if (!QuestManager.Instance.IsQuestActive(pitchRoundsQuest))
                QuestManager.Instance.StartQuest(pitchRoundsQuest);

            QuestManager.Instance.AddProgress(pitchRoundsQuest, 1);
        }

        // Decide which Prof dialogue to play
        if (minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedSuccess)
        {
            if (pitchSuccessQuest != null)
            {
                if (!QuestManager.Instance.IsQuestActive(pitchSuccessQuest))
                    QuestManager.Instance.StartQuest(pitchSuccessQuest);

                QuestManager.Instance.AddProgress(pitchSuccessQuest, 1);
            }

            if (profOnSuccessDialogue != null)
                dialogueManager.StartDialogue(profOnSuccessDialogue);
        }
        else if (minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedFail)
        {
            if (pitchFailQuest != null)
            {
                if (!QuestManager.Instance.IsQuestActive(pitchFailQuest))
                    QuestManager.Instance.StartQuest(pitchFailQuest);

                QuestManager.Instance.AddProgress(pitchFailQuest, 1);
            }

            if (profOnFailDialogue != null)
                dialogueManager.StartDialogue(profOnFailDialogue);
        }
        else
        {
            // Normal “after round” talk
            if (profAfterRoundDialogue != null)
                dialogueManager.StartDialogue(profAfterRoundDialogue);
        }
    }

    // Call this from your pitch-area “Start” button/trigger when the player comes back for next round
    public void StartOrContinueMinigame()
    {
        if (minigameCanvas != null) minigameCanvas.enabled = true;

        if (minigameManager.State == IdeationPitchMinigameManager.GameState.NotStarted)
            minigameManager.StartMinigame();
        else if (minigameManager.State == IdeationPitchMinigameManager.GameState.WaitingAfterRound)
            minigameManager.StartNextRound();

        minigameUI.RefreshUI();
    }
}
