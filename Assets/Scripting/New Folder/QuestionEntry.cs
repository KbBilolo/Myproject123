using UnityEngine;

[System.Serializable]
public class QuestionEntry
{
    [TextArea] public string questionText;

    // Optional future-proofing
    public string category; // "Risk", "Profit", "Innovation"
}
