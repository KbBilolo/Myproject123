using UnityEngine;

public class PitchAreaTriggerBox : MonoBehaviour
{
    [Header("Reference")]
    public PitchFlowBridge pitchBridge;
    public IdeationPitchMinigameManager minigameManager; // ADD THIS

    [Header("Options")]
    public bool triggerOnce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // BLOCK re-entry if game already finished
        if (minigameManager != null &&
            (minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedSuccess ||
             minigameManager.State == IdeationPitchMinigameManager.GameState.CompletedFail))
        {
            Debug.Log("[PitchTrigger] Pitch already completed. Ignoring trigger.");
            return;
        }

        if (pitchBridge == null)
        {
            Debug.LogError("PitchAreaTriggerBox: pitchBridge is not assigned.");
            return;
        }

        pitchBridge.StartOrContinueMinigame();
    }
}
