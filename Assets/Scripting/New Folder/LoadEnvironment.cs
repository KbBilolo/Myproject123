using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEnvironment : MonoBehaviour
{
    public static LoadEnvironment Instance;

    [System.Serializable]
    public class EnvironmentEntry
    {
        public string id;
        public GameObject environmentPrefab;
    }

    [Header("Registered Environments")]
    public List<EnvironmentEntry> environmentList = new List<EnvironmentEntry>();
    private Dictionary<string, GameObject> environmentDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        environmentDictionary = new Dictionary<string, GameObject>();

        foreach (var env in environmentList)
        {
            if (!environmentDictionary.ContainsKey(env.id))
            {
                environmentDictionary.Add(env.id, env.environmentPrefab);
            }
        }
    }
    
    public void LoadEnvironmentById(string id)
    {
        if (environmentDictionary.ContainsKey(id))
        {
            Instantiate(environmentDictionary[id]);
        }
        else
        {
            Debug.LogWarning($"Environment with id '{id}' not found.");
        }
    }

    public void UnloadEnvironmentById(string id)
    {
        // Implementation for unloading environment
    }
}
