using UnityEngine;

public class BookPickup : MonoBehaviour
{
    public QuestData bookQuest;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        QuestManager.Instance.AddProgress(bookQuest, 1);
        Destroy(gameObject);
    }
}
