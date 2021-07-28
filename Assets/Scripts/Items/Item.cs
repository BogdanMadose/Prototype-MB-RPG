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

    public virtual string GetDescription()
    {
        string color = string.Empty;
        switch (quality)
        {
            case Quality.Normal:
                color = "#FFFFFF";
                break;
            case Quality.Magic:
                color = "#001CF1";
                break;
            case Quality.Rare:
                color = "#E2D904";
                break;
            case Quality.Epic:
                color = "#8622F3";
                break;
            case Quality.Unique:
                color = "#DB4523";
                break;
        }
        return string.Format("<color={0}>{1}</color>", color, title);
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
