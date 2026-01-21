using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Minigame1Manager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI ideasLeftText;
    public TextMeshProUGUI scoreText;

    [Header("Card")]
    public GameObject cardPrefab;
    public Transform cardParent;

    [Header("Config")]
    public float timeLimit = 25f;
    public int correctPoints = 10;
    public int wrongPenalty = 5;

    private float time;
    private int index;
    private int score;

    // TEMP test data
    private List<(string text, bool isGood)> ideas =
        new List<(string, bool)>
    {
        ("AI study planner for students", true),
        ("Uber but for pigeons", false),
        ("Budget app for freelancers", true),
        ("Gambling Game for Children", false),
        ("Rebranding Same Products Online", false),
        ("Sunglass text writer for the deaf", true),
        ("Portable fan for the PWD", true),
        ("AI Garbage tracker from CCTVs", true),
        ("Crypto café for cats", false)
    };

    void Start()
    {
        time = timeLimit;
        score = 0;
        UpdateScoreUI();
        SpawnCard();
    }

    void Update()
    {
        time -= Time.deltaTime;
        timerText.text = Mathf.Ceil(time) + "s";

        if (time <= 0)
        {
            Debug.Log("TIME UP");
            enabled = false;
        }
    }

    void SpawnCard()
    {
        // If no more ideas, end game
        if (index >= ideas.Count)
        {
            Debug.Log("MINIGAME COMPLETE");
            ideasLeftText.text = "Ideas Left: 0";
            enabled = false;
            return;
        }

        // Update ideas left BEFORE spawning
        ideasLeftText.text = "Ideas Left: " + (ideas.Count - index);

        var card = Instantiate(cardPrefab, cardParent);
        card.GetComponent<SwipeIdeaCard>()
            .Init(ideas[index].text, ideas[index].isGood, this);

        index++;
    }


    public void ResolveSwipe(bool swipedRight, bool isGood)
    {
        bool correct = (swipedRight && isGood) || (!swipedRight && !isGood);

        if (correct)
        {
            score += correctPoints;
        }
        else
        {
            score -= wrongPenalty;
        }

        UpdateScoreUI();
        SpawnCard();
    }


    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
}
