using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionButtonData
{
    public ActionButtonData(string action, bool isItem, int index)
    {
        Action = action;
        IsItem = isItem;
        Index = index;
    }

    public string Action { get; set; }
    public bool IsItem { get; set; }
    public int Index { get; set; }
}
