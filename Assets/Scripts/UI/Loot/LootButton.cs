using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Tooltip("Loot icon")]
    [SerializeField] private Image icon;
    [Tooltip("Loot item name")]
    [SerializeField] private Text title;
    private LootWindow lootWindow;

    public Image Icon => icon;
    public Text Title => title;
    public Item Loot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.Instance.AddItemToInventory(Loot))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(Loot);
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
