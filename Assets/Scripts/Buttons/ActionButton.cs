using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Icon under action button")]
    [SerializeField] private Image icon;
    [Tooltip("Stack size text under the action button")]
    [SerializeField] private Text stackSize;
    private Stack<IUsable> _usables = new Stack<IUsable>();
    private int _count;

    public Button Button { get; private set; }
    public IUsable Usable { get; set; }
    public Image Icon { get => icon; set => icon = value; }
    public int Count => _count;
    public Text StackText => stackSize;
    public Stack<IUsable> Usables
    {
        get => _usables;
        set
        {
            if (value.Count > 0)
            {
                Usable = value.Peek();
            }
            else
            {
                Usable = null;
            }
            _usables = value;
        }
    }

    void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
    }

    private void Start() => InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.Instance.Movable != null && HandScript.Instance.Movable is IUsable)
            {
                SetUsable(HandScript.Instance.Movable as IUsable);
            }
        }
    }

    /// <summary>
    /// Call on click
    /// </summary>
    public void OnClick()
    {
        if (HandScript.Instance.Movable == null)
        {
            if (Usable != null)
            {
                Usable.Use();
            }
            else if (Usables != null && Usables.Count > 0)
            {
                Usables.Peek().Use();
            }
        }
    }


    /// <summary>
    /// Connect the item from inventory to the action bar and makes it usable
    /// </summary>
    /// <param name="usable">Usable item</param>
    public void SetUsable(IUsable usable)
    {
        if (usable is Item)
        {
            Usables = InventoryScript.Instance.GetUsables(usable);
            InventoryScript.Instance.FromSlot.Icon.color = Color.white;
            InventoryScript.Instance.FromSlot = null;
        }
        else
        {
            Usables.Clear();
            this.Usable = usable;
        }
        _count = Usables.Count;
        UpdateVisual();
        UIManager.Instance.RefreshToolTip(Usable as IDescribable);
    }


    /// <summary>
    /// Update the visuals of the item/spell that is dragged into the action bar
    /// </summary>
    public void UpdateVisual()
    {
        Icon.sprite = HandScript.Instance.PutItem().Icon;
        Icon.color = Color.white;
        if (_count > 1)
        {
            UIManager.Instance.UpdateStackSize(this);
        }
        else if (Usable is Spell)
        {
            UIManager.Instance.ClearStackSizeCount(this);
        }
    }


    /// <summary>
    /// Update the stack number of an item
    /// </summary>
    /// <param name="item">Stackable item</param>
    public void UpdateItemCount(Item item)
    {
        if (item is IUsable && Usables.Count > 0)
        {
            if (Usables.Peek().GetType() == item.GetType())
            {
                Usables = InventoryScript.Instance.GetUsables(item as IUsable);
                _count = Usables.Count;
                UIManager.Instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;
        if (Usable != null && Usable is IDescribable)
        {
            tmp = (IDescribable)Usable;
            UIManager.Instance.ShowToolTip(new Vector2(1, 0.5f), transform.position, tmp);
        }
        else if (Usables.Count > 0)
        {
            UIManager.Instance.ShowToolTip(new Vector2(1, 0.5f), transform.position, tmp);
        }
        if (tmp != null)
        {
            UIManager.Instance.ShowToolTip(new Vector2(1, 0.5f), transform.position, tmp);
        }
    }
}