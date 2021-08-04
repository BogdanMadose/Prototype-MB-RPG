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
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            AddItemToInventory(bag);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItemToInventory(potion);
        }
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
            AddItemToInventory((Equipment)Instantiate(items[11]));
        }
    }
    //============================================

    /// <summary>
    /// Handles adding a bag to a bag bar slot
    /// </summary>
    /// <param name="bag">Bag to be added</param>
    public void AddBagToBar(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                _bags.Add(bag);
                bag.BagButton = bagButton;
                break;
            }
        }
    }

    /// <summary>
    /// Handles adding a bag to a bag bar slot
    /// </summary>
    /// <param name="bag">Bag to be added</param>
    /// <param name="bagButton">Specific bag bar button</param>
    public void AddBagToBar(Bag bag, BagButton bagButton)
    {
        _bags.Add(bag);
        bagButton.Bag = bag;
    }

    /// <summary>
    /// Handles removing bag from bag bar
    /// </summary>
    /// <param name="bag">Bag to be removed</param>
    public void RemoveBagFromBar(Bag bag)
    {
        _bags.Remove(bag);
        Destroy(bag.BagScript.gameObject);
    }

    /// <summary>
    /// Handles switching bag items from inventory to bag bar
    /// </summary>
    /// <param name="oldBag">Bag to be switched</param>
    /// <param name="newBag">Bag to be switched with</param>
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
    /// Handles adding items to inventory slot
    /// </summary>
    /// <param name="item">Desired item</param>
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
    /// Handles stackable items in slots
    /// </summary>
    /// <param name="item">Desired item</param>
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
    /// Handles adding item to whole inventory
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
    /// Handles display of full inventory
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
    /// Handles decreasing and increasing stack size of usable items
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
}
