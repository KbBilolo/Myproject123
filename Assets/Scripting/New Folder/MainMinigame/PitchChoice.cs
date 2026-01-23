using UnityEngine;

[CreateAssetMenu(menuName = "Ideation/Pitch Choice")]
public class PitchChoice : ScriptableObject
{
    [TextArea]
    public string choiceText;

    public PitchStats statImpact;

    [TextArea]
    public string reactionText; // Shown after selection
}
