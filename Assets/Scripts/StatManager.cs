using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class StatManager : MonoBehaviour
{
    public static StatManager instance;

    public UnityEvent OnStatsChanged;

    public int Funds; //Cash available
    public int Popularity; //Charisma and public appeal
    public int Skills; //Technical ability and expertise
    public int Knowledge; //Understanding and information

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ModifyFunds(int amount)
    {
        Funds += amount;
        OnStatsChanged.Invoke();
    }
    public void ModifyPopularity(int amount)
    {
        Popularity += amount;
        OnStatsChanged.Invoke();
    }
    public void ModifySkills(int amount)
    {
        Skills += amount;
        OnStatsChanged.Invoke();
    }
    public void ModifyKnowledge(int amount)
    {
        Knowledge += amount;
        OnStatsChanged.Invoke();
    }


}
