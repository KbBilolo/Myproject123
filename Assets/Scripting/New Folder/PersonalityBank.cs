using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pitch/Personality Bank")]
public class PersonalityBank : ScriptableObject
{
    public List<PersonalityStats> personalities;
}
