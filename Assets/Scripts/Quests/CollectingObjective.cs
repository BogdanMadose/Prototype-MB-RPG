using System;

[Serializable]
public class CollectingObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (Type.ToLower() == item.Title.ToLower())
        {
            CurrentAmmount = InventoryScript.Instance.GetItemCount(item.Title);
            QuestLog.Instance.UpdateSelectedQuest();
            QuestLog.Instance.CheckCompletion();
        }
    }
}

