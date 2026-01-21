using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SwipeIdeaCard : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TextMeshProUGUI ideaText;
    public float swipeThreshold = 150f;

    private RectTransform rect;
    private Vector2 startPos;
    private bool isGood;
    private Minigame1Manager manager;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Init(string text, bool good, Minigame1Manager m)
    {
        ideaText.text = text;
        isGood = good;
        manager = m;
    }

    public void OnPointerDown(PointerEventData e)
    {
        startPos = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData e)
    {
        rect.anchoredPosition += e.delta;
    }

    public void OnPointerUp(PointerEventData e)
    {
        float dx = rect.anchoredPosition.x - startPos.x;

        if (Mathf.Abs(dx) > swipeThreshold)
        {
            manager.ResolveSwipe(dx > 0, isGood);
            Destroy(gameObject);
        }
        else
        {
            rect.anchoredPosition = startPos;
        }
    }
}
