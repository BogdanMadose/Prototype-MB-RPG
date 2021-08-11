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
    private VendorItems _vendorItem;

    /// <summary>
    /// Adds an item on page
    /// </summary>
    /// <param name="vendorItem">Item to be added</param>
    public void AddItemToVendorPage(VendorItems vendorItem)
    {
        this._vendorItem = vendorItem;
        if (vendorItem.Quantity > 0 || (vendorItem.Quantity == 0 && vendorItem.IsUnlimited))
        {
            icon.sprite = vendorItem.Item.Icon;
            itemName.text = string.Format("<color={0}>{1}</color>", QualityColor.Colors[vendorItem.Item.Quality], vendorItem.Item.Title);
            if (!vendorItem.IsUnlimited && vendorItem.Quantity > 1)
            {
                quantity.text = vendorItem.Quantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }
            if (vendorItem.Item.Price > 0)
            {
                price.text = "Price: " + vendorItem.Item.Price.ToString();
            }
            else
            {
                price.text = "Free";
            }
            gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Player.Instance.Gold >= _vendorItem.Item.Price) && InventoryScript.Instance.AddItemToInventory(Instantiate(_vendorItem.Item)))
        {
            BuyItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowToolTip(new Vector2(0, 1), transform.position, _vendorItem.Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }

    /// <summary>
    /// Item buying functionality
    /// </summary>
    private void BuyItem()
    {
        Player.Instance.Gold -= _vendorItem.Item.Price;
        if (!_vendorItem.IsUnlimited)
        {
            _vendorItem.Quantity--;
            quantity.text = _vendorItem.Quantity.ToString();
            if (_vendorItem.Quantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
