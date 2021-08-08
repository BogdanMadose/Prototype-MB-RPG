using UnityEngine;

public class VendorWindow : MonoBehaviour
{
    [Tooltip("Vendor window UI object")]
    [SerializeField] private CanvasGroup canvasGroup;
    [Tooltip("Interactable items in vendor window")]
    [SerializeField] private VendorButton[] vendorButtons;

    /// <summary>
    /// Closing vendor window functionality
    /// </summary>
    public void CloseVendorWindow()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Opening vendor window functionality
    /// </summary>
    public void OpenVendorWindow()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Create vendor window items
    /// </summary>
    /// <param name="vendorItems">List of items to be added</param>
    public void CreatePages(VendorItems[] vendorItems)
    {
        for (int i = 0; i < vendorItems.Length; i++)
        {
            vendorButtons[i].AddItemToVendorPage(vendorItems[i]);
        }
    }
}
