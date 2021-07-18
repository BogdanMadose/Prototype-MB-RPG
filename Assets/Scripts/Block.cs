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
    /// <summary>
    /// Invisible raycast blocker top
    /// </summary>
    [SerializeField] private GameObject first;
    /// <summary>
    /// Invisible raycast blocker bottom
    /// </summary>
    [SerializeField] private GameObject second;

    /// <summary>
    /// Deactivate raycast blockers
    /// </summary>
    #endregion

    #region Methods
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