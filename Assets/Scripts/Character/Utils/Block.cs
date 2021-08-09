using System;
using UnityEngine;

/// <summary>
/// Class responsable for holding raycast blocker game objects
/// <para>EDITOR EQUIVALENT - TLBlock, TRBlock, BLBlock, BRBlock</para>
/// <para>Change size and shape if wanted</para>
/// </summary>
[Serializable]
public class Block
{
    #region Variables
    [Tooltip("First blocking object")]
    [SerializeField] private GameObject first;
    [Tooltip("Second blocking object")]
    [SerializeField] private GameObject second;
    #endregion

    #region Methods
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
    #endregion
}