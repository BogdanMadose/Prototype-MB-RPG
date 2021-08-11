using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    private bool _complete = false;

    public Quest Quest { get; set; }

    public void SelectQuest()
    {
        GetComponent<Text>().color = Color.green;
        QuestLog.Instance.ShowQuestDescription(Quest);
    }

    public void DeselectQuest()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (Quest.IsComplete && !_complete)
        {
            _complete = true;
            GetComponent<Text>().text += "(Complete)";
        }
        else if (!Quest.IsComplete)
        {
            _complete = false;
            GetComponent<Text>().text = Quest.Title;
        }
    }
}
