using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Ideation/Pitch Question")]
public class PitchQuestion : ScriptableObject
{
    [TextArea]
    public string questionText;

    public List<PitchChoice> choices;
}
