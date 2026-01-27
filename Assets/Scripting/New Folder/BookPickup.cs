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
            bookVisual.SetActive(false); // Hide the book visual
            bookedInspected = true;


        QuestManager.Instance.AddProgress(bookQuest, 1);
        dialogueManager.StartDialogue(dialogueData, transform);
        // Optionally: keep this GameObject active for further logic
    }
}
