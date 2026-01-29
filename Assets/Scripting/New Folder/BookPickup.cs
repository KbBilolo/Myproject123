using UnityEngine;
using UnityEngine.UI;

public class BookPickup : MonoBehaviour
{
    public QuestData bookQuest;
    public GameObject inspectButton;
    public DialogueManager dialogueManager;
    public DialogueData dialogueData;
    public GameObject bookVisual; // Assign the visual book GameObject here
    public bool bookedInspected = false;

    [Header("Locking")]
    public bool requireQuestToBeActive = true;


    private bool playerInRange = false;

    void Start()
    {
        if (inspectButton != null)
            inspectButton.SetActive(false);
        if (bookVisual != null)
            bookVisual.SetActive(true);

        bookedInspected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        //  Gate: don't allow interaction until the quest giver started it
        if (requireQuestToBeActive)
        {
            if (bookQuest == null || !QuestManager.Instance.IsQuestActive(bookQuest))
            {
                // Player can be in range, but no inspect yet
                if (inspectButton != null) inspectButton.SetActive(false);
                return;
            }
        }

        if (inspectButton != null && bookedInspected == false)
        {
            inspectButton.SetActive(true);
            var button = inspectButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(Inspect);
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (inspectButton != null)
        {
            inspectButton.SetActive(false);
            var button = inspectButton.GetComponent<Button>();
            if (button != null)
                button.onClick.RemoveAllListeners();
        }
    }

    public void Inspect()
    {
        if (!playerInRange) return;

        if (inspectButton != null)
            inspectButton.SetActive(false);

        if (bookVisual != null)
        {
            bookVisual.SetActive(false);
        }
        bookedInspected = true;

        //  Ensure quest exists or started before adding progress
        if (bookQuest != null)
        {
            if (bookQuest != null && QuestManager.Instance.IsQuestActive(bookQuest))
            {
                QuestManager.Instance.AddProgress(bookQuest, 1);
            }

        }

        // Dialogue (optional)
        if (dialogueManager != null && dialogueData != null)
            dialogueManager.StartDialogue(dialogueData, transform);
    }

}
