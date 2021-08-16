using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour
{
    private static MessageFeedManager _instance;
    public static MessageFeedManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MessageFeedManager>();
            }
            return _instance;
        }
    }

    [Tooltip("Message prefab object")]
    [SerializeField] private GameObject messagePref;

    /// <summary>
    /// Write message on screen
    /// </summary>
    /// <param name="message">Writen message</param>
    public void WriteMessage(string message)
    {
        GameObject go = Instantiate(messagePref, transform);
        go.GetComponent<Text>().text = message;
        go.transform.SetAsFirstSibling();
        Destroy(go, 1.5f);
    }
}
