using System;

[Serializable]
public class BagData
{
    public BagData(int slotCount, int bagIndex)
    {
        SlotCount = slotCount;
        BagIndex = bagIndex;
    }

    public int SlotCount { get; set; }
    public int BagIndex { get; set; }
}
