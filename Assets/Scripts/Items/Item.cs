using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private int stackSize;
    [SerializeField] private Sprite icon;
    private SlotScript _slot;

    public int StackSize { get => stackSize; }
    public Sprite Icon { get => icon; }
    protected SlotScript Slot { get => _slot; set => _slot = value; }
}
