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
    [Tooltip("Item vendoring price")]
    [SerializeField] private int price;
    private SlotScript _slot;
    private CharacterButton _characterButton;

    public int StackSize => stackSize;
    public Sprite Icon => icon;
    public Quality Quality => quality;
    public string Title => title;
    public SlotScript Slot { get => _slot; set => _slot = value; }
    public CharacterButton CharacterButton
    {
        get => _characterButton;
        set
        {
            Slot = null;
            _characterButton = value;
        }
    }
    public int Price => price;

    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>", QualityColor.Colors[Quality], Title);
    }

    /// <summary>
    /// Remove item
    /// </summary>
    public void Remove()
    {
        if (Slot != null)
        {
            Slot.RemoveItemFromSlot(this);
        }
    }
}
