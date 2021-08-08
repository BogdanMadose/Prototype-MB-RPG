using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable
{
    [Tooltip("Vendor window reference")]
    [SerializeField] private VendorWindow vendorWindow;
    [Tooltip("Items the vendor can have")]
    [SerializeField] private VendorItems[] items;

    public void Interact()
    {
        vendorWindow.CreatePages(items);
        vendorWindow.OpenVendorWindow();
    }

    public void StopInteracting()
    {
        vendorWindow.CloseVendorWindow();
    }


}
