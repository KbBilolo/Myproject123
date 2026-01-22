using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialogue/StoryDialog")]
public class StoryDialogData : ScriptableObject
{
    public List<StoryDialogLine> lines;
}

[System.Serializable]
public class StoryDialogChoice
{
    public string choiceText;
    public int nextLineIndex = -1;
    public UnityEvent onChoiceSelected;
}

[System.Serializable]
public class StoryDialogLine
{
    public string speaker;
    public string text;

    public List<UnityEvent> onLineShownEvents = new List<UnityEvent>();

    // Add choices for branching
    public List<StoryDialogChoice> choices = new List<StoryDialogChoice>();

    public void InvokeEvents()
    {
        if (onLineShownEvents != null)
        {
            foreach (var unityEvent in onLineShownEvents)
            {
                unityEvent?.Invoke();
            }
        }
    }
}
