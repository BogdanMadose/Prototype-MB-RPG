using UnityEngine;

public class Vendor : NPC, IInteractable
{
    [Tooltip("Items the vendor can have")]
    [SerializeField] private VendorItems[] items;

    public VendorItems[] Items => items;
}
