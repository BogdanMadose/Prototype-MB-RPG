using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SaveData()
    {
        InventoryData = new InventoryData();
        ChestsData = new List<ChestsData>();
        EquipmentData = new List<EquipmentData>();
        ActionButtonData = new List<ActionButtonData>();
    }

    public PlayerData PlayerData { get; set; }
    public List<ChestsData> ChestsData { get; set; }
    public InventoryData InventoryData { get; set; }
    public List<EquipmentData> EquipmentData { get; set; }
    public List<ActionButtonData> ActionButtonData { get; set; }
}
