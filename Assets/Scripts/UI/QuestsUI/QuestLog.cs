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

    [Tooltip("Quest prefab")]
    [SerializeField] private GameObject questPrefab;
    [Tooltip("Quest list object")]
    [SerializeField] private Transform questList;
    [Tooltip("Quest description text")]
    [SerializeField] private Text questDescription;
    [Tooltip("Quest log UI display object")]
    [SerializeField] private CanvasGroup canvasGroup;
    [Tooltip("Quest count tracker text")]
    [SerializeField] private Text countText;
    [Tooltip("Quest log max quest count")]
    [SerializeField] private int questCount;
    private int _currentQCount;
    private Quest _selectedQuest;
    private List<QuestScript> _questScripts = new List<QuestScript>();
    private List<Quest> _quests = new List<Quest>();

    public List<Quest> Quests { get => _quests; set => _quests = value; }

    private void Start() => countText.text = _currentQCount + "/" + questCount;

    /// <summary>
    /// Accept quest
    /// </summary>
    /// <param name="quest">Accepted quest</param>
    public void AcceptQuest(Quest quest)
    {
        if (_currentQCount < questCount)
        {
            _currentQCount++;
            countText.text = _currentQCount + "/" + questCount;
            foreach (CollectingObjective co in quest.CollectingObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(co.UpdateItemCount);
                co.UpdateItemCount();
            }
            foreach (KillingObjective ko in quest.KillingObjectives)
            {
                GameManager.Instance.killConfirmedEvent += new KillConfirmed(ko.UpdateKillCount);
            }
            _quests.Add(quest);
            GameObject go = Instantiate(questPrefab, questList);
            QuestScript qs = go.GetComponent<QuestScript>();
            qs.Quest = quest;
            qs.Quest.QuestScript = qs;
            _questScripts.Add(qs);
            go.GetComponent<Text>().text = quest.Title;
            CheckCompletion();
        }
    }

    /// <summary>
    /// Abandon quest button
    /// </summary>
    public void AbandonQuest()
    {
        foreach (CollectingObjective co in _selectedQuest.CollectingObjectives)
        {
            InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChanged(co.UpdateItemCount);
            co.CollectingComplete();
        }
        foreach (KillingObjective ko in _selectedQuest.KillingObjectives)
        {
            GameManager.Instance.killConfirmedEvent -= new KillConfirmed(ko.UpdateKillCount);
        }
        RemoveQuest(_selectedQuest.QuestScript);
    }

    /// <summary>
    /// Check if quest is already active
    /// </summary>
    /// <param name="q">Accepted quest</param>
    public bool IsAccepted(Quest q) => _quests.Exists(x => x.Title == q.Title);

    /// <summary>
    /// Refresh selected quest
    /// </summary>
    public void UpdateSelectedQuest() => ShowQuestDescription(_selectedQuest);

    /// <summary>
    /// Display quest information
    /// </summary>
    /// <param name="quest">Selected quest</param>
    public void ShowQuestDescription(Quest quest)
    {
        if (quest != null)
        {
            if (_selectedQuest != null && _selectedQuest != quest)
            {
                _selectedQuest.QuestScript.UnHighlightQuest();
            }
            _selectedQuest = quest;
            string objective = string.Empty;
            string title = quest.Title;
            string description = quest.Description;
            foreach (Objective ob in quest.CollectingObjectives)
            {
                objective += ob.Type + ": " + ob.CurrentAmmount + " / " + ob.Ammount + "\n";
            }
            foreach (Objective ob in quest.KillingObjectives)
            {
                objective += ob.Type + ": " + ob.CurrentAmmount + " / " + ob.Ammount + "\n";
            }
            questDescription.text = string.Format("{0}\n\n<size=11>{1}</size>\n\nObjectives\n<size=11>{2}</size>", title, description, objective);
        }
    }

    /// <summary>
    /// Check quest completion
    /// </summary>
    public void CheckCompletion()
    {
        foreach (QuestScript qs in _questScripts)
        {
            qs.Quest.QuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    /// <summary>
    /// Remove quest
    /// </summary>
    /// <param name="qScript">Quest</param>
    public void RemoveQuest(QuestScript qScript)
    {
        _questScripts.Remove(qScript);
        Destroy(qScript.gameObject);
        _quests.Remove(qScript.Quest);
        questDescription.text = string.Empty;
        _selectedQuest = null;
        _currentQCount--;
        countText.text = _currentQCount + "/" + questCount;
        qScript.Quest.QuestGiver.UpdateQuestStatus();
        qScript = null;
    }

    /// <summary>
    /// Open / Close Quest Log
    /// </summary>
    public void OpenCloseQuestLog()
    {
        if (canvasGroup.alpha == 1)
        {
            CloseQuestLog();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// Close quest log button
    /// </summary>
    public void CloseQuestLog()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
