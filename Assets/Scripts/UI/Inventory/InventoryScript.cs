using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
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

    //==============DEBUGGING=====================
    [SerializeField] private Item[] items;

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
    }
    //============================================

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                _bags.Add(bag);
                break;
            }
        }
    }

    public void PlaceInEmptySlot(Item item)
    {
        foreach (Bag bag in _bags)
        {
            if (bag.BagScript.AddItemToBag(item))
            {
                return;
            }
        }
    }

    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in _bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (slot.StackItemInSlot(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddItemToInventory(Item item)
    {
        if (item.StackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }
        PlaceInEmptySlot(item);
    }

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
}
