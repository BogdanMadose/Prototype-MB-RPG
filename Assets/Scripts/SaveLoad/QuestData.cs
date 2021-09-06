using System;

[Serializable]
public class QuestData
{
    public QuestData(string title, string description, CollectingObjective[] collectingObjectives, KillingObjective[] killingObjectives, int questGiverID)
    {
        Title = title;
        Description = description;
        CollectingObjectives = collectingObjectives;
        KillingObjectives = killingObjectives;
        QuestGiverID = questGiverID;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public CollectingObjective[] CollectingObjectives { get; set; }
    public KillingObjective[] KillingObjectives { get; set; }
    public int QuestGiverID { get; set; }
}
