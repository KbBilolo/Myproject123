using UnityEngine;

[CreateAssetMenu(menuName = "Ideation/Crowd Profile")]
public class CrowdProfile : ScriptableObject
{
    public string crowdName;

    [Header("Stat Weights")]
    public float profitabilityWeight = 1f;
    public float riskWeight = 1f;
    public float publicAppealWeight = 1f;
    public float innovationWeight = 1f;

    [TextArea]
    public string hintText; // "Tonight's audience values stability and clarity."
}
