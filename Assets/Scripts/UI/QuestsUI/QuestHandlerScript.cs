using UnityEngine;

public class QuestHandlerScript : MonoBehaviour
{
    public Quest Quest { get; set; }

    /// <summary>
    /// Select quest
    /// </summary>
    public void Select()
    {
        QuestHandler.Instance.ShowQuestInfo(Quest);
    }
}
