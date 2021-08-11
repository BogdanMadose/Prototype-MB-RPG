using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questList;

    public void AcceptQuest(Quest quest)
    {
        GameObject go = Instantiate(questPrefab, questList);
        go.GetComponent<Text>().text = quest.Title;
    }
}
