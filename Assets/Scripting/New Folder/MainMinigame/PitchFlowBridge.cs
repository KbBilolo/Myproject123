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


    //I added this to stop the pitch minigame from blocking the players view when not in use.
    void Start()
    {
        // Initially hide the minigame UI
        if (minigameCanvas != null) minigameCanvas.enabled = false;
    }

    public void ReturnToProfessor()
    {
        // Close pitch UI
        if (minigameCanvas != null) minigameCanvas.enabled = false;

        // Mark: finished one round (only if we actually ended a round)
        if (pitchRoundsQuest != null)
        {
            if (!QuestManager.Instance.IsQuestActive(pitchRoundsQuest))
                QuestManager.Instance.StartQuest(pitchRoundsQuest);

            QuestManager.Instance.AddProgress(pitchRoundsQuest, 1);
        }

        // If minigame ended, mark success/fail quests
        if (minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedSuccess)
        {
            if (pitchSuccessQuest != null)
            {
                if (!QuestManager.Instance.IsQuestActive(pitchSuccessQuest))
                    QuestManager.Instance.StartQuest(pitchSuccessQuest);
                    

                QuestManager.Instance.AddProgress(pitchSuccessQuest, 1);
            }
        }
        else if (minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedFail)
        {
            if (pitchFailQuest != null)
            {
                if (!QuestManager.Instance.IsQuestActive(pitchFailQuest))
                    QuestManager.Instance.StartQuest(pitchFailQuest);

                QuestManager.Instance.AddProgress(pitchFailQuest, 1);
            }
        }

        // IMPORTANT: Do NOT start dialogue here.
        // Player must walk to Prof and press Talk.
        Debug.Log("[PitchFlowBridge] Round closed. Go talk to Prof manually.");
    }


    // Call this from your pitch-area “Start” button/trigger when the player comes back for next round
    public void StartOrContinueMinigame()
    {
        // If game already ended, don't reopen the minigame
        if (minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedSuccess ||
            minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedFail)
        {
            Debug.Log("[PitchFlowBridge] Minigame already finished. Go talk to Prof.");
            if (minigameCanvas != null) minigameCanvas.enabled = false; // safety
            return;
        }

        if (minigameCanvas != null) minigameCanvas.enabled = true;

        if (minigameManager.State == IdeationPitchMinigameManager.GameState.NotStarted)
            minigameManager.StartMinigame();
        else if (minigameManager.State == IdeationPitchMinigameManager.GameState.WaitingAfterRound)
            minigameManager.StartNextRound();

        minigameUI.ResetForNewRound();
        minigameUI.RefreshUI();
    }

}
