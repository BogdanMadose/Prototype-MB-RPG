using UnityEngine;

public abstract class Item : ScriptableObject, IMovable, IDescribable
{
    [Tooltip("Number of items permited in a stack")]
    [SerializeField] private int stackSize;
    [Tooltip("Item icon sprite")]
    [SerializeField] private Sprite icon;
    [Tooltip("Item name displayed on tooltip")]
    [SerializeField] private string title;
    [Tooltip("Item quality/rarity")]
    [SerializeField] private Quality quality;

    public int StackSize => stackSize;
    public Sprite Icon => icon;
    public SlotScript Slot { get; set; }
    public Quality Quality => quality;
    public string Title => title;

    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>", QualityColor.Colors[Quality], Title);
    }

    /// <summary>
    /// Removes item
    /// </summary>
    public void Remove()
    {
        if (Slot != null)
        {
            Slot.RemoveItemFromSlot(this);
        }
    }
}
