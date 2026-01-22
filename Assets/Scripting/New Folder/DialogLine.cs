using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string speaker;
    [TextArea(3, 6)]
    public string text;
}
