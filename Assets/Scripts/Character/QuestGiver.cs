using UnityEngine;

public class QuestGiver : NPC
{
    [Tooltip("Quests objects")]
    [SerializeField] private Quest[] quests;
    [Tooltip("Quest status related sprites")]
    [SerializeField] private Sprite questAvailable, questInProgress, questCompleted;
    [Tooltip("Quest status sprite renderer")]
    [SerializeField] private SpriteRenderer qsRenderer;

    public Quest[] Quests => quests;

    private void Start()
    {
        foreach (Quest q in quests)
        {
            q.QuestGiver = this;
        }
    }

    /// <summary>
    /// Update sprite status renderer
    /// </summary>
    public void UpdateQuestStatus()
    {
        int count = 0;
        foreach (Quest q in quests)
        {
            if (q != null)
            {
                if (q.IsComplete && QuestLog.Instance.IsAccepted(q))
                {
                    qsRenderer.sprite = questCompleted;
                    break;
                }
                else if (!QuestLog.Instance.IsAccepted(q))
                {
                    qsRenderer.sprite = questAvailable;
                    break;
                }
                else if (!q.IsComplete && QuestLog.Instance.IsAccepted(q))
                {
                    qsRenderer.sprite = questInProgress;
                }
            }
            else
            {
                count++;
                if (count == quests.Length)
                {
                    qsRenderer.enabled = false;
                }
            }
        }
    }
}
