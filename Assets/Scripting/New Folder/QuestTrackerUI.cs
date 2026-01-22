using UnityEngine;
using TMPro;

public class QuestTrackerUI : MonoBehaviour
{
    public static QuestTrackerUI Instance;

    public GameObject panel;
    public TextMeshProUGUI questText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowQuest(QuestData quest)
    {
        panel.SetActive(true);
        UpdateQuest(quest);
    }

    public void UpdateQuest(QuestData quest)
    {
        int progress = QuestManager.Instance.GetProgress(quest);
        questText.text =
            $"{quest.questName}\n{progress} / {quest.requiredAmount}";
    }

    public void HideQuest()
    {
        panel.SetActive(false);
    }
}
