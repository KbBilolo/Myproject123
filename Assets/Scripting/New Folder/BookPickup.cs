using UnityEngine;
using UnityEngine.UI;

public class BookPickup : MonoBehaviour
{
    public QuestData bookQuest;
    public GameObject inspectButton; // Assign the UI button in the Inspector
    public DialogueManager dialogueManager;
    public DialogueData dialogueData;

    private bool playerInRange = false;

    void Start()
    {
        if (inspectButton != null)
            inspectButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (inspectButton != null)
        {
            inspectButton.SetActive(true);
            // Remove previous listeners to avoid stacking
            inspectButton.GetComponent<Button>().onClick.RemoveAllListeners();
            inspectButton.GetComponent<Button>().onClick.AddListener(Inspect);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (inspectButton != null)
        {
            inspectButton.SetActive(false);
            inspectButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    // Called by the Inspect button
    public void Inspect()
    {
        if (!playerInRange) return;

        if (inspectButton != null)
            inspectButton.SetActive(false);

        QuestManager.Instance.AddProgress(bookQuest, 1);
        dialogueManager.StartDialogue(dialogueData, transform);
        Destroy(gameObject);
    }
}
