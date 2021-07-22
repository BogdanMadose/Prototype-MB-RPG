using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
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
        if (Usable != null)
        {
            Usable.Use();
        }
    }

    public void SetUsable(IUsable usable)
    {
        this.Usable = usable;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        Icon.sprite = HandScript.Instance.PutItem().Icon;
        Icon.color = Color.white;
    }
}