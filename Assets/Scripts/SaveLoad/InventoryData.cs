using System;
using System.Collections.Generic;

[Serializable]
public class InventoryData
{
    public List<BagData> Bags { get; set; }
    public List<ItemData> Items { get; set; }

    public InventoryData()
    {
        Bags = new List<BagData>();
        Items = new List<ItemData>();
    }
}
