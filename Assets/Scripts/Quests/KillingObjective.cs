using System;          

[Serializable]
public class KillingObjective : Objective
{
    /// <summary>
    /// Update kill count objective
    /// </summary>
    public void UpdateKillCount(Character character)
    {
        if (Type == character.Type)
        {
            CurrentAmmount++;
            QuestLog.Instance.UpdateSelectedQuest();
            QuestLog.Instance.CheckCompletion();
        }
    }
}

