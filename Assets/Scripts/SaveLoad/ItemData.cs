using System;

[Serializable]
public class ItemData
{
    public ItemData(string title, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        Title = title;
        StackCount = stackCount;
        SlotIndex = slotIndex;
        BagIndex = bagIndex;
    }

    public string Title { get; set; }
    public int StackCount { get; set; }
    public int SlotIndex { get; set; }
    public int BagIndex { get; set; }

}
