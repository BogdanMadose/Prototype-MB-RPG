using System;

[Serializable]
public class CollectingObjective : Objective
{
    /// <summary>
    /// Update item count
    /// </summary>
    /// <param name="item">Quest item</param>
    public void UpdateItemCount(Item item)
    {
        if (Type.ToLower() == item.Title.ToLower())
        {
            CurrentAmmount = InventoryScript.Instance.GetItemCount(item.Title);
            QuestLog.Instance.UpdateSelectedQuest();
            QuestLog.Instance.CheckCompletion();
        }
    }

    /// <summary>
    /// Update item count
    /// </summary>
    public void UpdateItemCount()
    {
        CurrentAmmount = InventoryScript.Instance.GetItemCount(Type);
        QuestLog.Instance.UpdateSelectedQuest();
        QuestLog.Instance.CheckCompletion();
    }
}

