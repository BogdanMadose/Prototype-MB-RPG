using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    [SerializeField] private Image icon;
    [SerializeField] private Text stackText;
    private ObservableStack<Item> _items = new ObservableStack<Item>();

    public bool IsEmpty => _items.Count == 0;
    public Item Item => !IsEmpty ? _items.Peek() : null;
    public Image Icon { get => icon; set => icon = value; }
    public int Count => _items.Count;
    public Text StackText => stackText;
    public bool IsSlotFull => !IsEmpty && Count >= Item.StackSize;

    private void Awake()
    {
        _items.OnPop += new UpdateStackEvent(UpdateSlot);
        _items.OnPush += new UpdateStackEvent(UpdateSlot);
        _items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItemToSlot(Item item)
    {
        _items.Push(item);
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
            _items.Pop();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot == null && !IsEmpty)
            {
                HandScript.Instance.TakeMovable(Item as IMovable);
                InventoryScript.Instance.FromSlot = this;
            }
            else if (InventoryScript.Instance.FromSlot != null)
            {
                if (PutItemBack()
                    || MergeItems(InventoryScript.Instance.FromSlot)
                    || SwapItems(InventoryScript.Instance.FromSlot)
                    || AddItemsToSlot(InventoryScript.Instance.FromSlot._items))
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
        if (!IsEmpty && item.name == Item.name && _items.Count < Item.StackSize)
        {
            _items.Push(item);
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
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from._items);
            from._items.Clear();
            from.AddItemsToSlot(_items);
            _items.Clear();
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
                AddItemToSlot(from._items.Pop());
            }
            return true;
        }
        return false;
    }

    public void TrashItem()
    {
        if (_items.Count > 0)
        {
            _items.Clear();
        }
    }

    private void UpdateSlot() => UIManager.Instance.UpdateStackSize(this);
}
