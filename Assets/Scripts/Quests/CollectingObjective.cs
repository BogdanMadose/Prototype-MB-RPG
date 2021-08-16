using System;
using System.Collections.Generic;

[Serializable]
public class CollectingObjective : Objective
{
    /// <summary>
    /// Update item count objective
    /// </summary>
    /// <param name="item">Quest item</param>
    public void UpdateItemCount(Item item)
    {
        if (Type.ToLower() == item.Title.ToLower())
        {
            CurrentAmmount = InventoryScript.Instance.GetItemCount(item.Title);
            if (CurrentAmmount <= Ammount)
            {
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1} / {2}", item.Title, CurrentAmmount, Ammount));
            }       
            QuestLog.Instance.UpdateSelectedQuest();
            QuestLog.Instance.CheckCompletion();
        }
    }

    /// <summary>
    /// Update item count objective
    /// </summary>
    public void UpdateItemCount()
    {
        CurrentAmmount = InventoryScript.Instance.GetItemCount(Type);
        QuestLog.Instance.UpdateSelectedQuest();
        QuestLog.Instance.CheckCompletion();
    }

    /// <summary>
    /// Remove item upon completion
    /// </summary>
    public void CollectingComplete()
    {
        Stack<Item> items = InventoryScript.Instance.GetItemsCount(Type, Ammount);
        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}

