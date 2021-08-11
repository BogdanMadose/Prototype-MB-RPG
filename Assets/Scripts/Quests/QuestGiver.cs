using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [Tooltip("Quests objects")]
    [SerializeField] private Quest[] quests;
    //--------------------------------------------
    [SerializeField] private QuestLog tmpLog;

    private void Awake()
    {
        tmpLog.AcceptQuest(quests[0]);
        tmpLog.AcceptQuest(quests[1]);
    }
}
