using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/StoryDialog")]
public class DialogData : ScriptableObject
{
    public List<DialogLine> lines;
}
