using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable
{
    [Tooltip("Vendor window reference")]
    [SerializeField] private VendorWindow vendorWindow;
    [Tooltip("Items the vendor can have")]
    [SerializeField] private VendorItems[] items;

    public bool IsOpened { get; set; }

    public void Interact()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            vendorWindow.CreatePages(items);
            vendorWindow.OpenVendorWindow(this);
        }
    }

    public void StopInteracting()
    {
        if (IsOpened)
        {
            IsOpened = false;
            vendorWindow.CloseVendorWindow();
        }
    }


}
