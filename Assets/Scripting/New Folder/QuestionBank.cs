using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pitch/Question Bank")]
public class QuestionBank : ScriptableObject
{
    public List<QuestionEntry> questions;
}
