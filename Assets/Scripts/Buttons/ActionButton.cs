using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    public Button MButton { get; private set; }
    public IUsable MUsable { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        MButton = GetComponent<Button>();
        MButton.onClick.AddListener(OnClick);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnClick()
    {
        if (MUsable != null)
        {
            MUsable.Use();
        }
    }
}
