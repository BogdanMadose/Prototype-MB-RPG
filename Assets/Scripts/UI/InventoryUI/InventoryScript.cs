using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;
    private static InventoryScript _instance;

    public static InventoryScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryScript>();
            }
            return _instance;
        }
    }

    [Tooltip("Items in all inventory")]
    [SerializeField] private Item[] items;
    [Tooltip("Number of bag button bar buttons")]
    [SerializeField] private BagButton[] bagButtons;
    private List<Bag> _bags = new List<Bag>();
    private SlotScript _fromSlot;

    public bool IsFull => !(_bags.Count < 5);
    public SlotScript FromSlot
    {
        get => _fromSlot;
        set
        {
            _fromSlot = value;
            if (value != null)
            {
                _fromSlot.Icon.color = Color.gray;
            }
        }
    }

    public int EmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in _bags)
            {
                count += bag.BagScript.EmptyBagSlotCount;
            }
            return count;
        }
    }

    public int TotalSlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in _bags)
            {
                count += bag.BagScript.Slots.Count;
            }
            return count;
        }
    }

    public int FullSlotCount => TotalSlotCount - EmptySlotCount;

    //==============DEBUGGING=====================
    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(20);
        bag.Use();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    Bag bag = (Bag)Instantiate(items[0]);
        //    bag.Initialize(20);
        //    bag.Use();
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Bag bag = (Bag)Instantiate(items[0]);
        //    bag.Initialize(10);
        //    AddItemToInventory(bag);
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    HealthPotion potion = (HealthPotion)Instantiate(items[1]);
        //    AddItemToInventory(potion);
        //}
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddItemToInventory((Equipment)Instantiate(items[2]));
            AddItemToInventory((Equipment)Instantiate(items[3]));
            AddItemToInventory((Equipment)Instantiate(items[4]));
            AddItemToInventory((Equipment)Instantiate(items[5]));
            AddItemToInventory((Equipment)Instantiate(items[6]));
            AddItemToInventory((Equipment)Instantiate(items[7]));
            AddItemToInventory((Equipment)Instantiate(items[8]));
            AddItemToInventory((Equipment)Instantiate(items[9]));
            AddItemToInventory((Equipment)Instantiate(items[10]));
            //AddItemToInventory((Equipment)Instantiate(items[11]));
        }
    }
    //============================================

    /// <summary>
    /// Add bag to bag bar
    /// </summary>
    /// <param name="bag">Added bag</param>
    public void AddBagToBar(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                _bags.Add(bag);
                bag.BagButton = bagButton;
                bag.BagScript.transform.SetSiblingIndex(bagButton.BagIndex);
                break;
            }
        }
    }

    /// <summary>
    /// Add bag to bag bar
    /// </summary>
    /// <param name="bag">Added bag</param>
    /// <param name="bagButton">Bag bar button/param>
    public void AddBagToBar(Bag bag, BagButton bagButton)
    {
        _bags.Add(bag);
        bagButton.Bag = bag;
        bag.BagScript.transform.SetSiblingIndex(bagButton.BagIndex);
    }

    /// <summary>
    /// Remove bag from bag bar
    /// </summary>
    /// <param name="bag">Bag removed</param>
    public void RemoveBagFromBar(Bag bag)
    {
        _bags.Remove(bag);
        Destroy(bag.BagScript.gameObject);
    }

    /// <summary>
    /// Switch bags
    /// </summary>
    /// <param name="oldBag">Bag in slot</param>
    /// <param name="newBag">Bag in inventory</param>
    public void SwapBagsFromBar(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (TotalSlotCount - oldBag.Slots) + newBag.Slots;
        if (newSlotCount - FullSlotCount >= 0)
        {
            List<Item> bagItems = oldBag.BagScript.GetItems();
            RemoveBagFromBar(oldBag);
            newBag.BagButton = oldBag.BagButton;
            newBag.Use();
            foreach (Item item in bagItems)
            {
                if (item != newBag)
                {
                    AddItemToInventory(item);
                }
            }
            AddItemToInventory(oldBag);
            HandScript.Instance.DropItem();
            Instance._fromSlot = null;
        }
    }

    /// <summary>
    /// Place item in empty bag slots
    /// </summary>
    /// <param name="item">Item added</param>
    public bool PlaceInEmptySlot(Item item)
    {
        foreach (Bag bag in _bags)
        {
            if (bag.BagScript.AddItemToBag(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Stack items
    /// </summary>
    /// <param name="item">Item added</param>
    /// <returns>
    /// <para>TRUE - item is stackable / can be placed in stack</para>
    /// <para>FALSE - item is not stackable / cannot be placed in stack</para>
    /// </returns>
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in _bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (slot.StackItemInSlot(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Add item to empty inventory slots
    /// </summary>
    /// <param name="item">Desired item</param>
    public bool AddItemToInventory(Item item)
    {
        if (item.StackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        return PlaceInEmptySlot(item);
    }

    /// <summary>
    /// Open / close all bags
    /// </summary>
    public void OpenCloseInventory()
    {
        bool closedBag = _bags.Find(x => !x.BagScript.IsOpen);
        foreach (Bag bag in _bags)
        {
            if (bag.BagScript.IsOpen != closedBag)
            {
                bag.BagScript.OpenClose();
            }
        }
    }

    /// <summary>
    /// Update stack size
    /// </summary>
    /// <param name="type">Type of usable item</param>
    /// <returns>All usable items</returns>
    public Stack<IUsable> GetUsables(IUsable type)
    {
        Stack<IUsable> usables = new Stack<IUsable>();
        foreach (Bag bag in _bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.Item.GetType() == type.GetType())
                {
                    foreach (Item item in slot.Items)
                    {
                        usables.Push(item as IUsable);
                    }
                }
            }
        }
        return usables;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

    /// <summary>
    /// Update item stack count
    /// </summary>
    /// <param name="type">Item type</param>
    /// <returns>Item count</returns>
    public int GetItemCount(string type)
    {
        int itemCount = 0;
        foreach (Bag bag in _bags)
        {
            foreach (SlotScript ss in bag.BagScript.Slots)
            {
                if (!ss.IsEmpty && ss.Item.Title == type)
                {
                    itemCount += ss.Items.Count;
                }
            }
        }
        return itemCount;
    }

    /// <summary>
    /// Check if more quest items are needed
    /// </summary>
    /// <param name="type">Item type</param>
    /// <param name="count">Item count</param>
    /// <returns>Items</returns>
    public Stack<Item> GetItemsCount(string type, int count)
    {
        Stack<Item> tmpItems = new Stack<Item>();
        foreach (Bag bag in _bags)
        {
            foreach (SlotScript ss in bag.BagScript.Slots)
            {
                if (!ss.IsEmpty && ss.Item.Title == type)
                {
                    foreach (Item item in ss.Items)
                    {
                        tmpItems.Push(item);
                        if (tmpItems.Count == count)
                        {
                            return tmpItems;
                        }
                    }
                }
            }
        }
        return tmpItems;
    }
}
