using System;
using System.Collections.Generic;

[Serializable]
public class QuestGiverData
{
    public QuestGiverData(List<string> completedQuests, int questGiverID)
    {
        CompletedQuests = completedQuests;
        QuestGiverID = questGiverID;
    }

    public List<string> CompletedQuests { get; set; }
    public int QuestGiverID { get; set; }
}
