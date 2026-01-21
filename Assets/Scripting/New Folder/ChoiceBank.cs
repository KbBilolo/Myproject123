using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pitch/Choice Bank")]
public class ChoiceBank : ScriptableObject
{
    public List<DialogChoice> choices;
}
