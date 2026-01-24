using UnityEngine;

public class PitchAreaTriggerBox : MonoBehaviour
{
    

    [Header("Reference")]
    public PitchFlowBridge pitchBridge;

    [Header("Options")]
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (triggerOnce && hasTriggered) return;
        hasTriggered = true;

        if (pitchBridge == null)
        {
            Debug.LogError("PitchAreaTriggerBox: pitchBridge is not assigned in Inspector.");
            return;
        }

        pitchBridge.StartOrContinueMinigame();
    }
}
