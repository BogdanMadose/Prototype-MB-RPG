using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text stackSize;
    private Stack<IUsable> _usables = new Stack<IUsable>();
    private int _count;
    public Button Button { get; private set; }
    public IUsable Usable { get; set; }
    public Image Icon { get => icon; set => icon = value; }
    public int Count => _count;
    public Text StackText => stackSize;

    // Start is called before the first frame update
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

    public void OnClick()
    {
        if (HandScript.Instance.Movable == null)
        {
            if (Usable != null)
            {
                Usable.Use();
            }
            if (_usables != null && _usables.Count > 0)
            {
                _usables.Peek().Use();
            }
        }
    }

    public void SetUsable(IUsable usable)
    {
        if (usable is Item)
        {
            _usables = InventoryScript.Instance.GetUsables(usable);
            _count = _usables.Count;
            InventoryScript.Instance.FromSlot.Icon.color = Color.white;
            InventoryScript.Instance.FromSlot = null;
        }
        else
        {
            this.Usable = usable;
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        Icon.sprite = HandScript.Instance.PutItem().Icon;
        Icon.color = Color.white;
        if (_count > 1)
        {
            UIManager.Instance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUsable && _usables.Count > 0)
        {
            if (_usables.Peek().GetType() == item.GetType())
            {
                _usables = InventoryScript.Instance.GetUsables(item as IUsable);
                _count = _usables.Count;
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
            //UIManager.Instance.ShowToolTip(transform.position);
        }
        else if (_usables.Count > 0)
        {
            //UIManager.Instance.ShowToolTip(transform.position);
        }
        if (tmp != null)
        {
            UIManager.Instance.ShowToolTip(transform.position, tmp);
        }
    }
}