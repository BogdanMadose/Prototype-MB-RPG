using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Tooltip("Item icon")]
    [SerializeField] private Image icon;
    [Tooltip("Item name/title")]
    [SerializeField] private Text itemName;
    [Tooltip("Item price")]
    [SerializeField] private Text price;
    [Tooltip("Item quanity")]
    [SerializeField] private Text quantity;

    public void AddItemToVendorPage(VendorItems vendorItem)
    {
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
