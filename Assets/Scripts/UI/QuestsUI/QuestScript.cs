using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    private bool _complete = false;

    public Quest Quest { get; set; }

    /// <summary>
    /// Quest selected
    /// </summary>
    public void HighlightQuest()
    {
        GetComponent<Text>().color = Color.green;
        QuestLog.Instance.ShowQuestDescription(Quest);
    }

    /// <summary>
    /// Quest deselected
    /// </summary>
    public void UnHighlightQuest()
    {
        GetComponent<Text>().color = Color.white;
    }

    /// <summary>
    /// Check if quest is completed
    /// </summary>
    public void IsComplete()
    {
        if (Quest.IsComplete && !_complete)
        {
            _complete = true;
            GetComponent<Text>().text = "<color=#ffbb0f><size=18>? </size></color>" + Quest.Title;
            MessageFeedManager.Instance.WriteMessage(string.Format("{0} (Complete)", Quest.Title));
        }
        else if (!Quest.IsComplete)
        {
            _complete = false;
            GetComponent<Text>().text = "<color=#c0c0c0ff><size=18>? </size></color>" + Quest.Title;
        }
    }
}
