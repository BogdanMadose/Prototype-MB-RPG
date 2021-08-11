using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    private static QuestLog _instance;

    public static QuestLog Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestLog>();
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questList;
    [SerializeField] private Text questDescription;
    private Quest _selectedQuest;
    private List<QuestScript> _questScripts = new List<QuestScript>();

    public void AcceptQuest(Quest quest)
    {
        foreach (CollectingObjective co in quest.CollectingObjectives)
        {
            InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(co.UpdateItemCount);
        }
        GameObject go = Instantiate(questPrefab, questList);
        QuestScript qs = go.GetComponent<QuestScript>();
        qs.Quest = quest;
        qs.Quest.QuestScript = qs;
        _questScripts.Add(qs);
        go.GetComponent<Text>().text = quest.Title;
    }

    public void UpdateSelectedQuest()
    {
        ShowQuestDescription(_selectedQuest);
    }

    public void ShowQuestDescription(Quest quest)
    {
        if (quest != null)
        {
            if (_selectedQuest != null && _selectedQuest != quest)
            {
                _selectedQuest.QuestScript.DeselectQuest();
            }
            _selectedQuest = quest;
            string objective = string.Empty;
            string title = quest.Title;
            string description = quest.Description;
            foreach (Objective ob in quest.CollectingObjectives)
            {
                objective += ob.Type + ": " + ob.CurrentAmmount + " / " + ob.Ammount + "\n";
            }
            questDescription.text = string.Format("{0}\n\n<size=11>{1}</size>\n\nObjectives\n<size=11>{2}</size>", title, description, objective);
        }    
    }

    public void CheckCompletion()
    {
        foreach (QuestScript qs in _questScripts)
        {
            qs.IsComplete();
        }
    }
}
