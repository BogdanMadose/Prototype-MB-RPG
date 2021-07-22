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

    public Sprite Portrait => portrait;

    /// <summary>
    /// Deselect targeted NPC
    /// </summary>
    public virtual void Deselect()
    {
        healthChanged -= new HealthChanged(UIManager.Instance.UpdateTargetFrame);
        npcRemoved -= new NPCRemoved(UIManager.Instance.HideTargetFrame);
    }

    /// <summary>
    /// Select targeted NPC
    /// </summary>
    /// <returns>Hitbox of the targeted NPC</returns>
    public virtual Transform Select() => hitBox;

    /// <summary>
    /// Event triggerer for when health value is being changed
    /// </summary>
    /// <param name="health">Health value</param>
    public void OnHealthChanged(float health) => healthChanged?.Invoke(health);

    /// <summary>
    /// Event triggerer for when NPC is deselected
    /// </summary>
    public void OnNPCRemoved()
    {
        npcRemoved?.Invoke();
        Destroy(gameObject);
    }
}
