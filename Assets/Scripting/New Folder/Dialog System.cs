using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprovalCalculator : MonoBehaviour
{
    public int CalculateApproval(
        IdeaStats stats,
        NPCPreferences npc,
        DialogChoice choice)
    {
        float score = 0;

        score += stats.profitability * npc.profitabilityWeight;
        score += stats.innovation * npc.innovationWeight;
        score += stats.publicAppeal * npc.publicAppealWeight;
        score -= stats.risk * npc.riskWeight; // risk hurts

        score /= 100f; // normalize
        score += choice.baseApproval;

        return Mathf.RoundToInt(score);
    }

}
