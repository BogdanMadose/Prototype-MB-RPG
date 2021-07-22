using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    public Button MButton { get; private set; }
    public IUsable MUsable { get; set; }
    public Image MIcon { get => icon; set => icon = value; }

    // Start is called before the first frame update
    void Awake()
    {
        MButton = GetComponent<Button>();
        MButton.onClick.AddListener(OnClick);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MInstance.MMovable != null && HandScript.MInstance.MMovable is IUsable)
            {
                SetUsable(HandScript.MInstance.MMovable as IUsable);
            }
        }
    }

    public void OnClick()
    {
        if (MUsable != null)
        {
            MUsable.Use();
        }
    }

    public void SetUsable(IUsable usable)
    {
        this.MUsable = usable;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        MIcon.sprite = HandScript.MInstance.Put().MIcon;
        MIcon.color = Color.white;
    }
}