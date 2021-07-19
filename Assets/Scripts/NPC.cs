using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Health value change event
/// </summary>
/// <param name="health">Health value</param>
public delegate void HealthChanged(float health);

/// <summary>
/// NPC untargeted event
/// </summary>
public delegate void NPCRemoved();
public class NPC : Character
{
    public event HealthChanged healthChanged;
    public event NPCRemoved npcRemoved;

    [SerializeField] private Sprite portrait;

    public Sprite MPortrait { get => portrait; }

    /// <summary>
    /// Deselect targeted NPC
    /// </summary>
    public virtual void Deselect()
    {
        healthChanged -= new HealthChanged(UIManager.MInstance.UpdateTargetFrame);
        npcRemoved -= new NPCRemoved(UIManager.MInstance.HideTargetFrame);
    }

    /// <summary>
    /// Select targeted NPC
    /// </summary>
    /// <returns>Hitbox of the targeted NPC</returns>
    public virtual Transform Select()
    {
        return hitBox;
    }

    /// <summary>
    /// Event triggerer for when health value is being changed
    /// </summary>
    /// <param name="health">Health value</param>
    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    /// <summary>
    /// Event triggerer for when NPC is deselected
    /// </summary>
    public void OnNPCRemoved()
    {
        if (npcRemoved != null)
        {
            npcRemoved();
        }
        Destroy(gameObject);
    }
}
