using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    public int requiredAmount;
}
