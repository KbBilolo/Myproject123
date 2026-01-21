using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
    public NameBank nameBank;
    public PersonalityBank personalityBank;
    public QuestionBank questionBank;
    public ChoiceBank choiceBank;

    public GeneratedNPC GenerateNPC()
    {
        GeneratedNPC npc = new GeneratedNPC();

        npc.npcName = nameBank.names[Random.Range(0, nameBank.names.Count)];
        npc.personality = personalityBank.personalities[Random.Range(0, personalityBank.personalities.Count)];
        npc.question = questionBank.questions[Random.Range(0, questionBank.questions.Count)];
        npc.choices = GetRandomChoices(3);

        return npc;
    }

    private List<DialogChoice> GetRandomChoices(int count)
    {
        List<DialogChoice> result = new List<DialogChoice>();
        List<DialogChoice> pool = new List<DialogChoice>(choiceBank.choices);

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }
}
