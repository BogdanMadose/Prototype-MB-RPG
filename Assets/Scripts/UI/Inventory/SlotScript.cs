using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    [SerializeField] private Image icon;
    [SerializeField] private Text stackText;
    private ObservableStack<Item> _items = new ObservableStack<Item>();

    public bool IsEmpty => Items.Count == 0;
    public Item Item => !IsEmpty ? Items.Peek() : null;
    public Image Icon { get => icon; set => icon = value; }
    public int Count => Items.Count;
    public Text StackText => stackText;
    public bool IsSlotFull => !IsEmpty && Count >= Item.StackSize;
    public BagScript BagInSlot { get; set; }
    public ObservableStack<Item> Items => _items;

    private void Awake()
    {
        Items.OnPop += new UpdateStackEvent(UpdateSlot);
        Items.OnPush += new UpdateStackEvent(UpdateSlot);
        Items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItemToSlot(Item item)
    {
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.Slot = this;
        return true;
    }

    public bool AddItemsToSlot(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == Item.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsSlotFull)
                {
                    return false;
                }
                AddItemToSlot(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void RemoveItemFromSlot(Item item)
    {
        if (!IsEmpty)
        {
            Items.Pop();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.Instance.Movable != null && (HandScript.Instance.Movable is Bag))
                {
                    if (Item is Bag)
                    {
                        InventoryScript.Instance.SwapBagsFromBar(HandScript.Instance.Movable as Bag, Item as Bag);
                    }
                }
                else
                {
                    HandScript.Instance.TakeMovable(Item as IMovable);
                    InventoryScript.Instance.FromSlot = this;
                }
            }
            else if (InventoryScript.Instance.FromSlot == null && IsEmpty && (HandScript.Instance.Movable is Bag))
            {
                Bag bag = (Bag)HandScript.Instance.Movable;
                if (bag.BagScript != BagInSlot && (InventoryScript.Instance.EmptySlotCount - bag.Slots) > 0)
                {
                    AddItemToSlot(bag);
                    bag.BagButton.RemoveBag();
                    HandScript.Instance.DropItem();
                }
            }
            else if (InventoryScript.Instance.FromSlot != null)
            {
                if (PutItemBack()
                    || MergeItems(InventoryScript.Instance.FromSlot)
                    || SwapItems(InventoryScript.Instance.FromSlot)
                    || AddItemsToSlot(InventoryScript.Instance.FromSlot.Items))
                {
                    HandScript.Instance.DropItem();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItemInSlot();
        }
    }

    public void UseItemInSlot()
    {
        if (Item is IUsable)
        {
            (Item as IUsable).Use();
        }
    }

    public bool StackItemInSlot(Item item)
    {
        if (!IsEmpty && item.name == Item.name && Items.Count < Item.StackSize)
        {
            Items.Push(item);
            item.Slot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        if (InventoryScript.Instance.FromSlot == this)
        {
            InventoryScript.Instance.FromSlot.Icon.color = Color.white;
            return true;
        }
        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.Item.GetType() != Item.GetType() || from.Count + Count > Item.StackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.Items);
            from.Items.Clear();
            from.AddItemsToSlot(Items);
            Items.Clear();
            AddItemsToSlot(tmpFrom);
            return true;
        }
        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.Item.GetType() == Item.GetType() && !IsSlotFull)
        {
            int freeStackCount = Item.StackSize - Count;
            for (int i = 0; i < freeStackCount; i++)
            {
                AddItemToSlot(from.Items.Pop());
            }
            return true;
        }
        return false;
    }

    public void TrashItem()
    {
        if (Items.Count > 0)
        {
            Items.Clear();
        }
    }

    private void UpdateSlot() => UIManager.Instance.UpdateStackSize(this);
}
