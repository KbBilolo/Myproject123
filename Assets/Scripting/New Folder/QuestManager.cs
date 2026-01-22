using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<QuestData, int> questProgress = new();
    private HashSet<QuestData> completedQuests = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartQuest(QuestData quest)
    {
        if (questProgress.ContainsKey(quest)) return;

        questProgress[quest] = 0;
        Debug.Log("Quest Started: " + quest.questName);
    }

    public void AddProgress(QuestData quest, int amount = 1)
    {
        if (!questProgress.ContainsKey(quest)) return;
        if (completedQuests.Contains(quest)) return;

        questProgress[quest] += amount;

        Debug.Log($"Quest Progress [{quest.questName}]: {questProgress[quest]}/{quest.requiredAmount}");

        if (questProgress[quest] >= quest.requiredAmount)
        {
            completedQuests.Add(quest);
            Debug.Log("Quest Completed: " + quest.questName);
        }
    }

    public bool IsQuestActive(QuestData quest)
    {
        return questProgress.ContainsKey(quest);
    }

    public bool IsQuestCompleted(QuestData quest)
    {
        return completedQuests.Contains(quest);
    }

    public int GetProgress(QuestData quest)
    {
        return questProgress.ContainsKey(quest) ? questProgress[quest] : 0;
    }
}
