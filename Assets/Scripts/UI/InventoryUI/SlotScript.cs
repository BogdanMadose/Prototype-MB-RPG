using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Slot icon sprite")]
    [SerializeField] private Image icon;
    [Tooltip("Slot stack size text")]
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

    /// <summary>
    /// Handles adding an item to slot
    /// </summary>
    /// <param name="item">Desired item</param>
    /// <returns></returns>
    public bool AddItemToSlot(Item item)
    {
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.Slot = this;
        return true;
    }

    /// <summary>
    /// Handles adding multiple stackable items to slot
    /// </summary>
    /// <param name="newItems">Desired item stack</param>
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

    /// <summary>
    /// Handles removing of an item from slot
    /// </summary>
    /// <param name="item">Desired item</param>
    public void RemoveItemFromSlot(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.Instance.OnItemCountChanged(Items.Pop());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.Instance.Movable != null)
                {
                    if (HandScript.Instance.Movable is Bag)
                    {
                        if (Item is Bag)
                        {
                            InventoryScript.Instance.SwapBagsFromBar(HandScript.Instance.Movable as Bag, Item as Bag);
                        }
                    }
                    else if (HandScript.Instance.Movable is Equipment)
                    {
                        if (Item is Equipment && (Item as Equipment).ItemPlacement == (HandScript.Instance.Movable as Equipment).ItemPlacement)
                        {
                            (Item as Equipment).Equip();
                            HandScript.Instance.DropItem();
                        }
                    }

                }
                else
                {
                    HandScript.Instance.TakeMovable(Item as IMovable);
                    InventoryScript.Instance.FromSlot = this;
                }
            }
            else if (InventoryScript.Instance.FromSlot == null && IsEmpty)
            {
                if (HandScript.Instance.Movable is Bag)
                {
                    Bag bag = (Bag)HandScript.Instance.Movable;
                    if (bag.BagScript != BagInSlot && (InventoryScript.Instance.EmptySlotCount - bag.Slots) > 0)
                    {
                        AddItemToSlot(bag);
                        bag.BagButton.RemoveBag();
                        HandScript.Instance.DropItem();
                    }
                }
                else if (HandScript.Instance.Movable is Equipment)
                {
                    Equipment equipment = (Equipment)HandScript.Instance.Movable;
                    CharacterPannel.Instance.SelectedButton.DequipItem();
                    AddItemToSlot(equipment);
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
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.Movable == null)
        {
            UseItemInSlot();
        }
    }

    /// <summary>
    /// Handles using of items from slot
    /// </summary>
    public void UseItemInSlot()
    {
        if (Item is IUsable)
        {
            (Item as IUsable).Use();
        }
        else if (Item is Equipment)
        {
            (Item as Equipment).Equip();
        }
    }

    /// <summary>
    /// Handles stacking items in one slot
    /// </summary>
    /// <param name="item">Desired stackable items</param>
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

    /// <summary>
    /// Returns item to original slot
    /// </summary>
    private bool PutItemBack()
    {
        if (InventoryScript.Instance.FromSlot == this)
        {
            InventoryScript.Instance.FromSlot.Icon.color = Color.white;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles item switching
    /// </summary>
    /// <param name="from">Original item</param>
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

    /// <summary>
    /// Handles merging stackable items
    /// </summary>
    /// <param name="from">Original item</param>
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

    /// <summary>
    /// Handles destroying of item(s) from slot
    /// </summary>
    public void TrashItems()
    {
        int count = Items.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                InventoryScript.Instance.OnItemCountChanged(Items.Pop());
            }
        }
    }

    /// <summary>
    /// Handles updating stack size in slot
    /// </summary>
    private void UpdateSlot() => UIManager.Instance.UpdateStackSize(this);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.Instance.ShowToolTip(new Vector2(1, 0.5f), transform.position, Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }
}
