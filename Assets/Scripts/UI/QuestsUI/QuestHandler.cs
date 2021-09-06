using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : Window
{
    public static QuestHandler _instance;
    public static QuestHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestHandler>();
            }
            return _instance;
        }
    }

    [Tooltip("Quest prefab")]
    [SerializeField] private GameObject questGiverPrefab;
    [Tooltip("Quest display area reference")]
    [SerializeField] private Transform questArea;
    [Tooltip("Back button reference")]
    [SerializeField] private GameObject backBtn;
    [Tooltip("Accept button reference")]
    [SerializeField] private GameObject acceptBtn;
    [Tooltip("Complete button reference")]
    [SerializeField] private GameObject completeBtn;
    [Tooltip("Quest description area reference")]
    [SerializeField] private GameObject questDescription;
    private List<GameObject> _quests = new List<GameObject>();
    private QuestGiver _questGiver;
    private Quest _selected;

    /// <summary>
    /// Show available quests
    /// </summary>
    /// <param name="questGiver">Quest NPC</param>
    public void ShowQuests(QuestGiver questGiver)
    {
        _questGiver = questGiver;
        foreach (GameObject go in _quests)
        {
            Destroy(go);
        }
        questArea.gameObject.SetActive(true);
        questDescription.gameObject.SetActive(false);
        foreach (Quest quest in questGiver.Quests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questGiverPrefab, questArea);
                go.GetComponent<QuestHandlerScript>().Quest = quest;
                _quests.Add(go);
                go.GetComponent<Text>().text = "<color=#ffbb0f><size=18>! </size></color>" + quest.Title;
                if (QuestLog.Instance.IsAccepted(quest))
                {
                    Color c = go.GetComponent<Text>().color;
                    c.a = 0.5f;
                    go.GetComponent<Text>().color = c;
                    go.GetComponent<Text>().text = "<color=#c0c0c0ff><size=18>? </size></color>" + quest.Title;
                }
                if (QuestLog.Instance.IsAccepted(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text = "<color=#ffbb0f><size=18>? </size></color>" + quest.Title;
                }

            }
        }
    }

    public override void OpenWindow(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        base.OpenWindow(npc);
    }

    public override void CloseWindow()
    {
        completeBtn.SetActive(false);
        base.CloseWindow();
    }

    /// <summary>
    /// Display quest information
    /// </summary>
    /// <param name="quest">Selected quest</param>
    public void ShowQuestInfo(Quest quest)
    {
        _selected = quest;
        if (QuestLog.Instance.IsAccepted(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!QuestLog.Instance.IsAccepted(quest))
        {
            acceptBtn.SetActive(true);
        }
        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.gameObject.SetActive(true);
        string title = quest.Title;
        string description = quest.Description;
        //string objective = string.Empty;
        //foreach (Objective ob in quest.CollectingObjectives)
        //{
        //    objective += ob.Type + ": " + ob.CurrentAmmount + " / " + ob.Ammount + "\n";
        //}
        questDescription.GetComponent<Text>().text = string.Format("{0}\n\n<size=10>{1}</size>\n", title, description);
    }

    /// <summary>
    /// Go back to quest list
    /// </summary>
    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        completeBtn.SetActive(false);
        ShowQuests(_questGiver);
    }

    /// <summary>
    /// Accept quest button
    /// </summary>
    public void Accept()
    {
        QuestLog.Instance.AcceptQuest(_selected);
        Back();
    }

    /// <summary>
    /// Complete quest button
    /// </summary>
    public void Complete()
    {
        if (_selected.IsComplete)
        {
            for (int i = 0; i < _questGiver.Quests.Length; i++)
            {
                if (_selected == _questGiver.Quests[i])
                {
                    _questGiver.Completed.Add(_selected.Title);
                    _questGiver.Quests[i] = null;
                    _selected.QuestGiver.UpdateQuestStatus();
                }
            }
            foreach (CollectingObjective co in _selected.CollectingObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChanged(co.UpdateItemCount);
                co.CollectingComplete();
            }
            foreach (KillingObjective ko in _selected.KillingObjectives)
            {
                GameManager.Instance.killConfirmedEvent -= new KillConfirmed(ko.UpdateKillCount);
            }
            Player.Instance.GainXP(XPManager.CalculateXP(_selected));
            QuestLog.Instance.RemoveQuest(_selected.QuestScript);
            Back();
        }
    }
}
