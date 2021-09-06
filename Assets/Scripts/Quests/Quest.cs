using System;
using UnityEngine;

[Serializable]
public class Quest
{
    [Tooltip("Quest name")]
    [SerializeField] private string title;
    [Tooltip("Quest description")]
    [SerializeField] private string description;
    [Tooltip("Type: Collecting")]
    [SerializeField] private CollectingObjective[] collectingObjectives;
    [Tooltip("Type: Killing")]
    [SerializeField] private KillingObjective[] killingObjectives;
    [Tooltip("Quest level")]
    [SerializeField] private int lvl;
    [Tooltip("Quest xp")]
    [SerializeField] private int xp;

    public string Title { get => title; set => title = value; }
    public QuestScript QuestScript { get; set; }
    public string Description { get => description; set => description = value; }
    public CollectingObjective[] CollectingObjectives { get => collectingObjectives; set => collectingObjectives = value; }
    public QuestGiver QuestGiver { get; set; }
    public int Level { get => lvl; set => lvl = value; }
    public int Xp => xp;
    public KillingObjective[] KillingObjectives { get => killingObjectives; set => killingObjectives = value; }
    public bool IsComplete
    {
        get
        {
            foreach (Objective ob in CollectingObjectives)
            {
                if (!ob.IsComplete)
                {
                    return false;
                }
            }
            foreach (Objective ob in KillingObjectives)
            {
                if (!ob.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }


}
