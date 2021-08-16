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
            MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1} / {2}", character.Type, CurrentAmmount, Ammount));
            QuestLog.Instance.UpdateSelectedQuest();
            QuestLog.Instance.CheckCompletion();
        }
    }
}

