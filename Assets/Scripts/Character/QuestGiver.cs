using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [Tooltip("Quests objects")]
    [SerializeField] private Quest[] quests;
    [Tooltip("Quest status related sprites")]
    [SerializeField] private Sprite questAvailable, questInProgress, questCompleted;
    [Tooltip("Quest status sprite renderer")]
    [SerializeField] private SpriteRenderer qsRenderer;
    [Tooltip("Quest NPC ID")]
    [SerializeField] private int questGiverID;
    private List<string> completed = new List<string>();

    public Quest[] Quests => quests;
    public int QuestGiverID => questGiverID;
    public List<string> Completed
    {
        get => completed;
        set
        {
            completed = value;
            foreach (string title in completed)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].Title == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

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
