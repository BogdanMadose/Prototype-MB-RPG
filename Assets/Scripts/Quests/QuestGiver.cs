using UnityEngine;

public class QuestGiver : NPC
{
    [Tooltip("Quests objects")]
    [SerializeField] private Quest[] quests;

    public Quest[] Quests => quests;
}
