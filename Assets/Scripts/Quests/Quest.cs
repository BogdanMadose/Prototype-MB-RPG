using System;
using UnityEngine;

[Serializable]
public class Quest
{
    [SerializeField] private string title;

    public string Title { get => title; set => title = value; }
}
