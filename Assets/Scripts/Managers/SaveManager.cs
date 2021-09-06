using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [Tooltip("Items found in slots")]
    [SerializeField] private Item[] items;
    [Tooltip("Action bar buttons")]
    [SerializeField] private ActionButton[] actionButtons;
    [Tooltip("Save game slots")]
    [SerializeField] private SaveGame[] saveGameSlots;
    [Tooltip("Pop up window")]
    [SerializeField] private GameObject dialogue;
    [Tooltip("Pop up message")]
    [SerializeField] private Text dialogueText;
    private SaveGame _current;
    private CharacterButton[] _eq;
    private Chest[] _chests;
    private string _act;

    private void Awake()
    {
        _chests = FindObjectsOfType<Chest>();
        _eq = FindObjectsOfType<CharacterButton>();
        foreach (SaveGame sg in saveGameSlots)
        {
            ShowSavedFiles(sg);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveGameSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            Player.Instance.SetDefaults();
        }
    }

    /// <summary>
    /// Show save game slots
    /// </summary>
    /// <param name="savedGame">Saved game file</param>
    private void ShowSavedFiles(SaveGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }

    /// <summary>
    /// Show confirmation dialogue
    /// </summary>
    /// <param name="go">Save game slot</param>
    public void ShowDialogue(GameObject go)
    {
        _act = go.name;
        switch (_act)
        {
            case "Load":
                dialogueText.text = "Do you want to load this save game?";
                break;
            case "Save":
                dialogueText.text = "Do you want to save your game in this slot?\n(Override if not empty)";
                break;
            case "Delete":
                dialogueText.text = "Are you sure you want to delete this save game?";
                break;
        }
        _current = go.GetComponentInParent<SaveGame>();
        dialogue.SetActive(true);
    }

    /// <summary>
    /// Execute confirmed action
    /// </summary>
    public void ExecuteAction()
    {
        switch (_act)
        {
            case "Load":
                LoadScene(_current);
                break;
            case "Save":
                Save(_current);
                break;
            case "Delete":
                Delete(_current);
                break;
        }
        CloseDialogue();
    }

    /// <summary>
    /// Close confirmation dialogue
    /// </summary>
    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    /// <summary>
    /// Delete save game
    /// </summary>
    /// <param name="savedGame">Save game file</param>
    private void Delete(SaveGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();
    }

    /// <summary>
    /// Save game state
    /// </summary>
    /// <param name="savedGame">Save game file</param>
    private void Save(SaveGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);
            SaveData data = new SaveData();
            data.Scene = SceneManager.GetActiveScene().name;
            SaveEquipment(data);
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveChests(data);
            SaveAction(data);
            SaveQuestGiver(data);
            SaveQuests(data);
            bf.Serialize(file, data);
            file.Close();
            ShowSavedFiles(savedGame);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Save player data
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Save chest data
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Save bags data
    /// </summary>
    /// <param name="data">Save game data</param>
    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.Instance.Bags.Count; i++)
        {
            data.InventoryData.Bags.Add(new BagData(InventoryScript.Instance.Bags[i].SlotCount, InventoryScript.Instance.Bags[i].BagButton.BagIndex));
        }
    }

    /// <summary>
    /// Save equipped items data
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Save action bar items
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Save inventory items
    /// </summary>
    /// <param name="data">Save game data</param>
    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.Instance.GetAllItems();
        foreach (SlotScript slot in slots)
        {
            data.InventoryData.Items.Add(new ItemData(slot.Item.Title, slot.Items.Count, slot.Index, slot.BagInSlot.BagIndex));
        }
    }

    /// <summary>
    /// Save quests
    /// </summary>
    /// <param name="data">Save game data</param>
    private void SaveQuests(SaveData data)
    {
        foreach (Quest q in QuestLog.Instance.Quests)
        {
            data.QuestData.Add(new QuestData(q.Title, q.Description, q.CollectingObjectives, q.KillingObjectives, q.QuestGiver.QuestGiverID));
        }
    }

    /// <summary>
    /// Save quest activity
    /// </summary>
    /// <param name="data">Save game data</param>
    private void SaveQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestGiver qg in questGivers)
        {
            data.QuestGiverData.Add(new QuestGiverData(qg.Completed, qg.QuestGiverID));
        }
    }

    /// <summary>
    /// Load game state
    /// </summary>
    /// <param name="savedGame">Save game file</param>
    private void Load(SaveGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            LoadEquipment(data);
            LoadBags(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadChests(data);
            LoadAction(data);
            LoadQuestGiver(data);
            LoadQuests(data);
        }
        catch (Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
        }
    }

    /// <summary>
    /// Load player data
    /// </summary>
    /// <param name="data">Save game data</param>
    private void LoadPlayer(SaveData data)
    {
        Player.Instance.Level = data.PlayerData.Level;
        Player.Instance.UpdateLevel();
        Player.Instance.Health.Initialize(data.PlayerData.Health, data.PlayerData.MaxHealth);
        Player.Instance.Mana.Initialize(data.PlayerData.Mana, data.PlayerData.MaxMana);
        Player.Instance.Xp.Initialize(data.PlayerData.XP, data.PlayerData.MaxXP);
        Player.Instance.transform.position = new Vector2(data.PlayerData.X, data.PlayerData.Y);
    }

    /// <summary>
    /// Load chests data
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Load bags data
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Load equipped items
    /// </summary>
    /// <param name="data">Save game data</param>
    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.EquipmentData)
        {
            CharacterButton cb = Array.Find(_eq, x => x.name == equipmentData.Type);
            cb.EquipItem(Array.Find(items, x => x.Title == equipmentData.Title) as Equipment);
        }
    }

    /// <summary>
    /// Load action bar items
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Load inventory items
    /// </summary>
    /// <param name="data">Save game data</param>
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

    /// <summary>
    /// Load quests
    /// </summary>
    /// <param name="data">Save game data</param>
    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestData questData in data.QuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.QuestGiverID == questData.QuestGiverID);
            Quest q = Array.Find(qg.Quests, x => x.Title == questData.Title);
            q.QuestGiver = qg;
            q.KillingObjectives = questData.KillingObjectives;
            QuestLog.Instance.AcceptQuest(q);
        }
    }

    /// <summary>
    /// Load quest activity
    /// </summary>
    /// <param name="data">Save game data</param>
    private void LoadQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
        foreach (QuestGiverData qgd in data.QuestGiverData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.QuestGiverID == qgd.QuestGiverID);
            qg.Completed = qgd.CompletedQuests;
            qg.UpdateQuestStatus();
        }
    }

    private void LoadScene(SaveGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            PlayerPrefs.SetInt("Load", savedGame.Index);
            SceneManager.LoadScene(data.Scene);
        }
    }
}