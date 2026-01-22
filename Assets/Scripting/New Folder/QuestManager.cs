using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private HashSet<QuestData> activeQuests = new();

    void Awake()
    {
        Instance = this;
    }

    public void StartQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest)) return;

        activeQuests.Add(quest);
        Debug.Log("Quest Started: " + quest.questName);
    }

    public bool IsQuestActive(QuestData quest)
    {
        return activeQuests.Contains(quest);
    }
}
