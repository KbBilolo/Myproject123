using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;

    [TextArea(2, 4)]
    public string text;

    public DialogueChoice[] choices;

    [Header("Quest Trigger")]
    public QuestData startQuest; // optional
}


[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueData nextDialogue;
    public QuestData startQuest; // optional
}

