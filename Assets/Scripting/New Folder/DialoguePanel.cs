using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePanel : MonoBehaviour
{
    public TMP_Text npcNameText;
    public TMP_Text questionText;
    public TMP_Text resultText;

    public List<Button> choiceButtons;

    public NPCGenerator npcGenerator;
    public ApprovalCalculator dialogSystem;
    public IdeaStats ideaStats;

    private GeneratedNPC currentNPC;

    void Start()
    {
        GenerateNewNPC();
    }

    public void GenerateNewNPC()
    {
        resultText.text = "";
        currentNPC = npcGenerator.GenerateNPC();

        npcNameText.text = currentNPC.npcName;
        questionText.text = currentNPC.question.questionText;

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            int index = i;
            // Use TMP_Text for button label
            TMP_Text btnText = choiceButtons[i].GetComponentInChildren<TMP_Text>();
            if (btnText != null)
                btnText.text = currentNPC.choices[i].choiceText;

            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() =>
                OnChoiceSelected(currentNPC.choices[index]));
        }
    }

    void OnChoiceSelected(DialogChoice choice)
    {
        // Apply stat changes
        ideaStats.profitability += choice.profitabilityChange;
        ideaStats.innovation += choice.innovationChange;
        ideaStats.publicAppeal += choice.publicAppealChange;
        ideaStats.risk += choice.riskChange;

        ClampStats();

        int approval = dialogSystem.CalculateApproval(
            ideaStats,
            ConvertPersonality(currentNPC.personality),
            choice
        );

        resultText.text = $"Approval Change: {approval}%";

        Invoke(nameof(GenerateNewNPC), 1.5f);
    }

    void ClampStats()
    {
        ideaStats.profitability = Mathf.Clamp(ideaStats.profitability, 0, 100);
        ideaStats.innovation = Mathf.Clamp(ideaStats.innovation, 0, 100);
        ideaStats.publicAppeal = Mathf.Clamp(ideaStats.publicAppeal, 0, 100);
        ideaStats.risk = Mathf.Clamp(ideaStats.risk, 0, 100);
    }

    NPCPreferences ConvertPersonality(PersonalityStats p)
    {
        return new NPCPreferences
        {
            profitabilityWeight = p.profitabilityWeight,
            innovationWeight = p.innovationWeight,
            publicAppealWeight = p.publicAppealWeight,
            riskWeight = p.riskWeight
        };
    }
}
