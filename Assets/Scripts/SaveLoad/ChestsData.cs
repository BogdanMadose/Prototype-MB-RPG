using System;
using System.Collections.Generic;

[Serializable]
public class ChestsData
{
    public ChestsData(string name)
    {
        Name = name;
        Items = new List<ItemData>();
    }

    public string Name { get; set; }
    public List<ItemData> Items { get; set; }
}
