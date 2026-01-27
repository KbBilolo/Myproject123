using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IdeationPitchUIController_Slider : MonoBehaviour
{
    [Header("Manager")]
    public IdeationPitchMinigameManager manager;

    [Header("TopBar")]
    public TextMeshProUGUI crowdNameText;
    public TextMeshProUGUI crowdHintText; // optional
    public TextMeshProUGUI approvalLabelText;
    public Slider approvalSlider;

    [Header("Stage")]
    public TextMeshProUGUI questionText;

    [Header("Choices")]
    public Button[] choiceButtons;          // 3 buttons
    public TextMeshProUGUI[] choiceTexts;   // 3 TMP texts

    [Header("Confirm")]
    public Button confirmButton;

    [Header("Feedback")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI reactionText;

    [Header("Mood")]
    public GameObject moodPanel;
    public Image moodIconImage;
    public Sprite moodVeryGood;
    public Sprite moodGood;
    public Sprite moodNeutral;
    public Sprite moodBad;

    [Header("Round Summary")]
    public GameObject roundSummaryPanel;
    public TextMeshProUGUI summaryTitleText;
    public TextMeshProUGUI summaryBullet1Text;
    public TextMeshProUGUI summaryBullet2Text;
    public Button summaryContinueButton;

    [Header("When summary closes, return to Prof/Story")]
    public UnityEvent onReturnToProfessor; // hook this in Inspector

    [Header("Timing")]
    public float approvalAnimDuration = 0.5f;
    public float reactionDelay = 0.25f;

    private int selectedChoiceIndex = -1;
    private bool locked = false;

    void Start()
    {
        // If your flow starts the minigame here:
        // manager.StartMinigame();
        // Otherwise call StartMinigame from Prof and just call RefreshUI() after.

        BindChoiceButtons();

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirm);

        summaryContinueButton.onClick.RemoveAllListeners();
        summaryContinueButton.onClick.AddListener(OnSummaryContinue);

        RefreshUI();
        SetLocked(false);
    }

    void BindChoiceButtons()
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int idx = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() =>
            {
                if (locked) return;
                selectedChoiceIndex = idx;
                confirmButton.interactable = true;
            });
        }
    }

    void SetLocked(bool value)
    {
        locked = value;
        for (int i = 0; i < choiceButtons.Length; i++)
            choiceButtons[i].interactable = !value;

        confirmButton.interactable = !value && selectedChoiceIndex != -1;
    }

    public void RefreshUI()
    {
        crowdNameText.text = manager.CrowdLabel;
        if (crowdHintText != null) crowdHintText.text = ""; // fill later

        // Slider setup
        approvalSlider.minValue = 0f;
        approvalSlider.maxValue = 100f; // keep as 0..100 for approval %
        approvalSlider.value = manager.Approval;
        approvalLabelText.text = $"{manager.Approval:0}% / {manager.TargetApproval:0}%";

        var q = manager.GetCurrentQuestion();
        if (q != null)
        {
            questionText.text = q.prompt;
            for (int i = 0; i < 3; i++)
                choiceTexts[i].text = q.choices[i].text;
        }

        feedbackPanel.SetActive(false);
        if (moodPanel != null) moodPanel.SetActive(false);
        roundSummaryPanel.SetActive(false);

        selectedChoiceIndex = -1;
        confirmButton.interactable = false;
    }

    void OnConfirm()
    {
        if (locked) return;
        if (selectedChoiceIndex < 0) return;

        SetLocked(true);

        var result = manager.ConfirmChoice(selectedChoiceIndex);
        StartCoroutine(FeedbackSequence(result));
    }

    IEnumerator FeedbackSequence(IdeationPitchMinigameManager.ConfirmResult result)
    {
        // Mood icon immediately
        if (moodPanel != null)
        {
            moodPanel.SetActive(true);
            moodIconImage.sprite = PickMoodSprite(result.delta);
        }

        // Animate slider value
        float from = result.approvalBefore;
        float to = result.approvalAfter;

        float t = 0f;
        while (t < approvalAnimDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / approvalAnimDuration);
            approvalSlider.value = Mathf.Lerp(from, to, a);
            yield return null;
        }
        approvalSlider.value = to;
        approvalLabelText.text = $"{to:0}% / {manager.TargetApproval:0}%";

        // Reaction after a short delay
        yield return new WaitForSeconds(reactionDelay);
        feedbackPanel.SetActive(true);
        reactionText.text = result.reaction;

        // If round ended: show summary overlay (and STOP there)
        if (result.roundEnded)
        {
            yield return new WaitForSeconds(0.35f);
            ShowRoundSummary(result);
            yield break;
        }

        // Otherwise: allow next question immediately
        yield return new WaitForSeconds(0.35f);
        feedbackPanel.SetActive(false);
        if (moodPanel != null) moodPanel.SetActive(false);

        selectedChoiceIndex = -1;
        SetLocked(false);
        RefreshUI(); // pulls next question
    }

    void ShowRoundSummary(IdeationPitchMinigameManager.ConfirmResult result)
    {
        roundSummaryPanel.SetActive(true);

        // Title like: "Round 1 Summary"
        summaryTitleText.text = $"Round {result.roundJustFinished} Summary";

        // Bullet mapping: keep it simple now, refine later
        summaryBullet1Text.text = result.roundSummary ?? "You received mixed feedback from the room.";
        summaryBullet2Text.text = $"Approval: {result.approvalAfter:0}% (Target: {manager.TargetApproval:0}%)";

        // Lock gameplay UI behind overlay
        SetLocked(true);
    }

    void OnSummaryContinue()
    {
        // Hide minigame UI and return to professor/story sequence
        roundSummaryPanel.SetActive(false);

        // If game ended, you likely go to Prof for final outcome dialogue
        // If it didn't end, you also go to Prof and later call manager.StartNextRound()

        onReturnToProfessor?.Invoke();
    }
    public void ResetForNewRound()
    {
        // Unlock input
        selectedChoiceIndex = -1;
        locked = false;

        // Hide overlays
        feedbackPanel.SetActive(false);
        if (moodPanel != null) moodPanel.SetActive(false);
        roundSummaryPanel.SetActive(false);

        // Reset buttons
        confirmButton.interactable = false;
        for (int i = 0; i < choiceButtons.Length; i++)
            choiceButtons[i].interactable = true;
    }


    Sprite PickMoodSprite(float delta)
    {
        if (delta >= 4f) return moodVeryGood != null ? moodVeryGood : moodGood;
        if (delta >= 1f) return moodGood != null ? moodGood : moodNeutral;
        if (delta > -1f) return moodNeutral;
        return moodBad != null ? moodBad : moodNeutral;
    }
}
