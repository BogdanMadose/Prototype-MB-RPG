using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    private Stack<IUsable> _usables;
    private int _count;
    public Button Button { get; private set; }
    public IUsable Usable { get; set; }
    public Image Icon { get => icon; set => icon = value; }

    // Start is called before the first frame update
    void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
    }

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
                _usables.Pop().Use();
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
    }
}