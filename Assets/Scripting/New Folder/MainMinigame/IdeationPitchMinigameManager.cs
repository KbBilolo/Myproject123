using System;
using System.Collections.Generic;
using UnityEngine;

public class IdeationPitchMinigameManager : MonoBehaviour
{
    public enum Stat { Profitability, Risk, PublicAppeal, Innovation }
    public enum CrowdType { RiskTakers, LongTermInvestors, CasualObservers, ROIFocused }

    [Serializable]
    public struct StatDelta
    {
        public int profitability;
        public int risk;
        public int publicAppeal;
        public int innovation;

        public StatDelta(int p, int r, int a, int i)
        { profitability = p; risk = r; publicAppeal = a; innovation = i; }
    }

    [Serializable]
    public class Choice
    {
        public string text;
        public StatDelta delta;
        public string reaction;

        public Choice(string text, StatDelta delta, string reaction)
        { this.text = text; this.delta = delta; this.reaction = reaction; }
    }

    [Serializable]
    public class Question
    {
        public string prompt;
        public List<Choice> choices;

        public Question(string prompt, List<Choice> choices)
        { this.prompt = prompt; this.choices = choices; }
    }

    public enum GameState
    {
        NotStarted,
        InRound,
        WaitingAfterRound,   // round ended; show summary; return to Prof
        CompletedSuccess,
        CompletedFail
    }

    public struct ConfirmResult
    {
        public int roundJustFinished;

        public float approvalBefore;
        public float approvalAfter;
        public float delta;
        public string reaction;

        public bool roundEnded;
        public bool gameEnded;
        public bool success;
        public string roundSummary;
    }


    [Header("Minigame Config")]
    [SerializeField] private float startingApproval = 40f;
    [SerializeField] private float targetApproval = 70f;
    [SerializeField] private int roundsLimit = 4;
    [SerializeField] private int questionsPerRound = 3;

    [Header("Crowd")]
    [SerializeField] private CrowdType crowdA = CrowdType.LongTermInvestors;
    [SerializeField] private CrowdType crowdB = CrowdType.CasualObservers;

    [Header("Sample Questions (auto-built in Awake)")]
    [SerializeField] private bool buildSampleQuestions = true;

    private List<Question> questions;
    private Dictionary<CrowdType, Dictionary<Stat, float>> crowdWeights;

    // runtime
    private float approval;
    private int roundIndex;                 // 0..roundsLimit-1
    private int questionIndexInRound;       // 0..questionsPerRound-1
    private int globalQuestionIndex;        // cycles through questions list

    private Dictionary<Stat, int> emphasisCounts;
    public GameState State { get; private set; } = GameState.NotStarted;

    // --- Read-only getters for UI/Quest
    public float Approval => approval;
    public float TargetApproval => targetApproval;
    public int RoundNumber => roundIndex + 1;
    public int RoundsLimit => roundsLimit;
    public string CrowdLabel => $"Tonight’s audience: {crowdA} & {crowdB}";

    void Awake()
    {
        BuildCrowdWeights();
        if (buildSampleQuestions) BuildSampleQuestions();
    }

    // Call when Prof starts the pitch minigame the first time
    public void StartMinigame()
    {
        approval = startingApproval;
        roundIndex = 0;
        globalQuestionIndex = 0;
        StartNextRound();

        Debug.Log("=== IDEATION HUB MINIGAME START ===");
        Debug.Log(CrowdLabel);
        Debug.Log($"Target Approval: {targetApproval}% | Starting Approval: {approval}% | Rounds: {roundsLimit} | Q/Round: {questionsPerRound}");
        Debug.Log("--------------------------------------------------");
    }

    // Call when player returns for Round 2, 3, 4 (after talking to Prof / scenes)
    public void StartNextRound()
    {
        if (State == GameState.CompletedFail || State == GameState.CompletedSuccess)
            return;

        if (roundIndex >= roundsLimit)
        {
            State = GameState.CompletedFail;
            return;
        }

        questionIndexInRound = 0;
        emphasisCounts = new Dictionary<Stat, int>
        {
            { Stat.Profitability, 0 },
            { Stat.Risk, 0 },
            { Stat.PublicAppeal, 0 },
            { Stat.Innovation, 0 }
        };

        State = GameState.InRound;
    }

    public Question GetCurrentQuestion()
    {
        if (questions == null || questions.Count == 0) return null;
        if (globalQuestionIndex < 0 || globalQuestionIndex >= questions.Count) globalQuestionIndex = 0;
        return questions[globalQuestionIndex];
    }

    // UI calls this when player presses Confirm
    public ConfirmResult ConfirmChoice(int choiceIndex)
    {
        if (State != GameState.InRound)
            throw new InvalidOperationException($"Cannot confirm choice when State={State}.");

        var q = GetCurrentQuestion();
        if (q == null) throw new InvalidOperationException("No current question.");
        if (choiceIndex < 0 || choiceIndex >= q.choices.Count)
            throw new ArgumentOutOfRangeException(nameof(choiceIndex));

        var choice = q.choices[choiceIndex];

        float before = approval;
        float delta = CalculateApprovalDelta(choice.delta);
        approval += delta;

        TrackEmphasis(choice.delta);

        Debug.Log($"Round {RoundNumber} | Q{questionIndexInRound + 1}: {q.prompt}");
        Debug.Log($"CHOICE: {choice.text}");
        Debug.Log($"REACTION: {choice.reaction}");
        Debug.Log($"APPROVAL CHANGE: {delta:+0.0;-0.0;0.0}  =>  NOW: {approval:0.0}%");
        Debug.Log("--------------------------------------------------");

        // advance question pointer
        questionIndexInRound++;
        globalQuestionIndex++;
        if (globalQuestionIndex >= questions.Count) globalQuestionIndex = 0;

        bool roundEnded = questionIndexInRound >= questionsPerRound;
        int roundJustFinished = RoundNumber; // roundIndex + 1 at this moment

        bool gameEnded = false;
        bool success = false;
        string summary = null;

        if (roundEnded)
        {
            summary = BuildRoundSummaryText();
            Debug.Log($"=== ROUND {RoundNumber} SUMMARY ===");
            Debug.Log(summary);
            Debug.Log($"Approval now: {approval:0.0}% (Target: {targetApproval}%)");
            Debug.Log("==================================================");

            // Decide if game ends now or continues later
            if (approval >= targetApproval)
            {
                success = true;
                gameEnded = true;
                State = GameState.CompletedSuccess;
            }
            else
            {
                // Round completed but game not ended yet
                roundIndex++;

                if (roundIndex >= roundsLimit)
                {
                    success = false;
                    gameEnded = true;
                    State = GameState.CompletedFail;
                }
                else
                {
                    // Important: stop here and let quest/prof scenes happen
                    State = GameState.WaitingAfterRound;
                }
            }
        }

        return new ConfirmResult
        {
            roundJustFinished = roundJustFinished,

            approvalBefore = before,
            approvalAfter = approval,
            delta = delta,
            reaction = choice.reaction,
            roundEnded = roundEnded,
            gameEnded = gameEnded,
            success = success,
            roundSummary = summary
        };

    }

    private float CalculateApprovalDelta(StatDelta d)
    {
        var wA = crowdWeights[crowdA];
        var wB = crowdWeights[crowdB];

        float wProfit = (wA[Stat.Profitability] + wB[Stat.Profitability]) * 0.5f;
        float wRisk = (wA[Stat.Risk] + wB[Stat.Risk]) * 0.5f;
        float wAppeal = (wA[Stat.PublicAppeal] + wB[Stat.PublicAppeal]) * 0.5f;
        float wInnov = (wA[Stat.Innovation] + wB[Stat.Innovation]) * 0.5f;

        float score =
            d.profitability * wProfit +
            d.risk * wRisk +
            d.publicAppeal * wAppeal +
            d.innovation * wInnov;

        return score * 2.0f; // pacing knob
    }

    private void TrackEmphasis(StatDelta d)
    {
        if (d.profitability > 0) emphasisCounts[Stat.Profitability]++;
        if (d.publicAppeal > 0) emphasisCounts[Stat.PublicAppeal]++;
        if (d.innovation > 0) emphasisCounts[Stat.Innovation]++;
        if (d.risk > 0) emphasisCounts[Stat.Risk]++;
    }

    private string BuildRoundSummaryText()
    {
        // Very simple teaching summary based on what you emphasized most
        Stat top = Stat.Profitability;
        int topCount = -1;

        foreach (var kv in emphasisCounts)
        {
            if (kv.Value > topCount)
            {
                top = kv.Key;
                topCount = kv.Value;
            }
        }

        string note =
            (crowdA == CrowdType.CasualObservers || crowdB == CrowdType.CasualObservers)
                ? "Casual listeners respond best to clarity and user impact."
                : "This crowd rewards strong framing and coherence.";

        return $"You leaned most on: {top} (x{topCount}). {note}";
    }

    private void BuildCrowdWeights()
    {
        crowdWeights = new Dictionary<CrowdType, Dictionary<Stat, float>>();

        crowdWeights[CrowdType.LongTermInvestors] = new Dictionary<Stat, float>
        {
            { Stat.Profitability,  1.3f },
            { Stat.Risk,          -1.2f },
            { Stat.PublicAppeal,   0.4f },
            { Stat.Innovation,     0.5f }
        };

        crowdWeights[CrowdType.CasualObservers] = new Dictionary<Stat, float>
        {
            { Stat.Profitability,  0.5f },
            { Stat.Risk,          -0.6f },
            { Stat.PublicAppeal,   1.4f },
            { Stat.Innovation,     0.3f }
        };

        crowdWeights[CrowdType.ROIFocused] = new Dictionary<Stat, float>
        {
            { Stat.Profitability,  1.6f },
            { Stat.Risk,          -1.0f },
            { Stat.PublicAppeal,   0.2f },
            { Stat.Innovation,     0.2f }
        };

        crowdWeights[CrowdType.RiskTakers] = new Dictionary<Stat, float>
        {
            { Stat.Profitability,  0.6f },
            { Stat.Risk,           0.8f },
            { Stat.PublicAppeal,   0.3f },
            { Stat.Innovation,     1.5f }
        };
    }

    private void BuildSampleQuestions()
    {
        questions = new List<Question>
        {
            new Question(
                "What do you emphasize when introducing your idea?",
                new List<Choice>
                {
                    new Choice("This idea focuses on steady growth and long-term sustainability.",
                        new StatDelta(+2, -1, 0, +1),
                        "Several investors nod approvingly."),
                    new Choice("This product disrupts the market and challenges existing norms.",
                        new StatDelta(-1, +2, 0, +3),
                        "Excitement ripples through part of the room, others look uneasy."),
                    new Choice("This idea is designed to be accessible to everyone.",
                        new StatDelta(+1, 0, +3, -1),
                        "Casual attendees seem engaged and attentive.")
                }
            ),
            new Question(
                "An audience member asks about potential risks.",
                new List<Choice>
                {
                    new Choice("Every venture has risks, but careful planning minimizes them.",
                        new StatDelta(+1, -2, 0, 0),
                        "The room relaxes slightly."),
                    new Choice("High risk is necessary to achieve real innovation.",
                        new StatDelta(0, +2, 0, +2),
                        "A few people smile. Others exchange glances."),
                    new Choice("We’ve designed safeguards to protect users and investors.",
                        new StatDelta(+1, -1, +1, 0),
                        "Several attendees nod in approval.")
                }
            ),
            new Question(
                "How do you justify the idea’s value?",
                new List<Choice>
                {
                    new Choice("It creates consistent returns over time.",
                        new StatDelta(+3, 0, 0, 0),
                        "Investors lean forward."),
                    new Choice("It improves everyday life for its users.",
                        new StatDelta(0, 0, +3, 0),
                        "The crowd smiles and murmurs approvingly."),
                    new Choice("It pushes the industry forward.",
                        new StatDelta(0, +1, 0, +2),
                        "A mix of applause and hesitation.")
                }
            )
        };
    }
}
