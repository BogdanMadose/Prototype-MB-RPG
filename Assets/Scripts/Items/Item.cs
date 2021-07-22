using UnityEngine;

public abstract class Item : ScriptableObject, IMovable
{
    [SerializeField] private int stackSize;
    [SerializeField] private Sprite icon;

    public int StackSize => stackSize;
    public Sprite Icon => icon;
    public SlotScript Slot { get; set; }

    public void Remove()
    {
        if (Slot != null)
        {
            Slot.RemoveItemFromSlot(this);
        }
    }
}
