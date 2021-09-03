using System;

[Serializable]
public class EquipmentData
{
    public EquipmentData(string title, string type)
    {
        Title = title;
        Type = type;
    }

    public string Title { get; set; }
    public string Type { get; set; }
}
