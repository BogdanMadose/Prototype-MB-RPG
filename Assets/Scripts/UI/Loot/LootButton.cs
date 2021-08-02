using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text title;

    public Image Icon => icon;
    public Text Title => title;
    public Item Loot { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.Instance.AddItemToInventory(Loot))
        {
            gameObject.SetActive(false);
            UIManager.Instance.HideToolTip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowToolTip(transform.position, Loot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }
}
