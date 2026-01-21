using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [System.Serializable]
    public class UIEntry
    {
        public string id;
        public GameObject canvas;
    }

    [Header("Registered UIs")]
    public List<UIEntry> uiList = new List<UIEntry>();

    private Dictionary<string, GameObject> uiDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        uiDictionary = new Dictionary<string, GameObject>();

        foreach (var ui in uiList)
        {
            if (!uiDictionary.ContainsKey(ui.id))
            {
                uiDictionary.Add(ui.id, ui.canvas);
            }
        }
    }

    public void ShowUI(string id)
    {
        DisableAllUI();

        if (uiDictionary.ContainsKey(id))
        {
            uiDictionary[id].SetActive(true);
        }
        else
        {
            Debug.LogWarning($"UI with id '{id}' not found.");
        }
    }

    public void HideUI(string id)
    {
        if (uiDictionary.ContainsKey(id))
        {
            uiDictionary[id].SetActive(false);
        }
    }

    public void DisableAllUI()
    {
        foreach (var ui in uiDictionary.Values)
        {
            ui.SetActive(false);
        }
    }
}
