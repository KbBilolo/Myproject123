using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public DialogueCondition[] conditions;
}

[System.Serializable]
public class DialogueCondition
{
    public QuestData quest;
    public QuestState requiredState;
    public DialogueLine[] lines;
}

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;

    [TextArea(2, 4)]
    public string text;

    public DialogueChoice[] choices;


}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueData nextDialogue;
    public QuestData startQuest;
}
