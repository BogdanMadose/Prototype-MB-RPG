using System;
using UnityEngine;

[Serializable]
public class Block
{
    [Tooltip("First blocking object")]
    [SerializeField] private GameObject first;
    [Tooltip("Second blocking object")]
    [SerializeField] private GameObject second;

    /// <summary>
    /// Deactivate raycast blockers
    /// </summary>
    public void Deactivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    /// <summary>
    /// Activate raycast blockers
    /// </summary>
    public void Activate()
    {

        first.SetActive(true);
        second.SetActive(true);
    }
}