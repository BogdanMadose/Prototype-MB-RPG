using System;
using UnityEngine;

[Serializable]
public class VendorItems
{
    [Tooltip("Item")]
    [SerializeField] private Item item;
    [Tooltip("Item quantity")]
    [SerializeField] private int quantity;
    [Tooltip("Is always on stock")]
    [SerializeField] private bool IsUnlimited;

    public Item Item => item;
    public int Quantity { get => quantity; set => quantity = value; }
    public bool IsUnlimited1 => IsUnlimited;
}
