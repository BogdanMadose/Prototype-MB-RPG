using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Tooltip("Items found in slots")]
    [SerializeField] private Item[] items;
    [Tooltip("Action bar buttons")]
    [SerializeField] private ActionButton[] actionButtons;
    private CharacterButton[] _eq;
    private Chest[] _chests;

    private void Awake()
    {
        _chests = FindObjectsOfType<Chest>();
        _eq = FindObjectsOfType<CharacterButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Load();
        }
    }

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create);
            SaveData data = new SaveData();
            SaveEquipment(data);
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveChests(data);
            SaveAction(data);
            bf.Serialize(file, data);
            file.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.PlayerData = new PlayerData(Player.Instance.Level,
            Player.Instance.Xp.CurrentValue,
            Player.Instance.Xp.MaxValue,
            Player.Instance.Health.CurrentValue,
            Player.Instance.Health.MaxValue,
            Player.Instance.Mana.CurrentValue,
            Player.Instance.Mana.MaxValue,
            Player.Instance.transform.position);
    }

    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < _chests.Length; i++)
        {
            data.ChestsData.Add(new ChestsData(_chests[i].name));
            foreach (Item item in _chests[i].Items)
            {
                if (_chests[i].Items.Count > 0)
                {
                    data.ChestsData[i].Items.Add(new ItemData(item.Title, item.Slot.Items.Count, item.Slot.Index));
                }
            }
        }
    }

    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.Instance.Bags.Count; i++)
        {
            data.InventoryData.Bags.Add(new BagData(InventoryScript.Instance.Bags[i].SlotCount, InventoryScript.Instance.Bags[i].BagButton.BagIndex));
        }
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (CharacterButton characterButton in _eq)
        {
            if (characterButton.Equipment != null)
            {
                data.EquipmentData.Add(new EquipmentData(characterButton.Equipment.Title, characterButton.name));
            }
        }
    }

    private void SaveAction(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].Usable != null)
            {
                ActionButtonData abd;
                if (actionButtons[i].Usable is Spell)
                {
                    abd = new ActionButtonData((actionButtons[i].Usable as Spell).Name, false, i);
                }
                else
                {
                    abd = new ActionButtonData((actionButtons[i].Usable as Item).Title, true, i);
                }
                data.ActionButtonData.Add(abd);
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.Instance.GetAllItems();
        foreach (SlotScript slot in slots)
        {
            data.InventoryData.Items.Add(new ItemData(slot.Item.Title, slot.Items.Count, slot.Index, slot.BagInSlot.BagIndex));
        }
    }

    private void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            LoadEquipment(data);
            LoadBags(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadChests(data);
            LoadAction(data);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.Instance.Level = data.PlayerData.Level;
        Player.Instance.UpdateLevel();
        Player.Instance.Health.Initialize(data.PlayerData.Health, data.PlayerData.MaxHealth);
        Player.Instance.Mana.Initialize(data.PlayerData.Mana, data.PlayerData.MaxMana);
        Player.Instance.Xp.Initialize(data.PlayerData.XP, data.PlayerData.MaxXP);
        Player.Instance.transform.position = new Vector2(data.PlayerData.X, data.PlayerData.Y);
    }

    private void LoadChests(SaveData data)
    {
        foreach (ChestsData chests in data.ChestsData)
        {
            Chest c = Array.Find(_chests, x => x.name == chests.Name);
            foreach (ItemData itemData in chests.Items)
            {
                Item item = Array.Find(items, x => x.Title == itemData.Title);
                item.Slot = c.BagScriptC.Slots.Find(x => x.Index == itemData.SlotIndex);
                c.Items.Add(item);
            }
        }
    }

    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.InventoryData.Bags)
        {

            Bag newBag = (Bag)Instantiate(items[0]);
            newBag.Initialize(bagData.SlotCount);
            InventoryScript.Instance.AddBagToBar(newBag, bagData.BagIndex);
            InventoryScript.Instance.OpenCloseInventory();
        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.EquipmentData)
        {
            CharacterButton cb = Array.Find(_eq, x => x.name == equipmentData.Type);
            cb.EquipItem(Array.Find(items, x => x.Title == equipmentData.Title) as Equipment);
        }
    }

    private void LoadAction(SaveData data)
    {
        foreach (ActionButtonData abd in data.ActionButtonData)
        {
            if (abd.IsItem)
            {
                actionButtons[abd.Index].SetUsable(InventoryScript.Instance.GetUsable(abd.Action));
            }
            else
            {
                actionButtons[abd.Index].SetUsable(SpellBook.Instance.GetSpell(abd.Action));
            }
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.InventoryData.Items)
        {
            Item item = Array.Find(items, x => x.Title == itemData.Title);
            for (int i = 0; i < itemData.StackCount; i++)
            {
                InventoryScript.Instance.PlaceInSpecificSlot(item, itemData.SlotIndex, itemData.BagIndex);
            }
        }
    }
}
