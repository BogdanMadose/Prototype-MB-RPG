using System;
using UnityEngine;

[Serializable]
public class Loot
{
    [Tooltip("Item dropped display")]
    [SerializeField] private Item item;
    [Tooltip("Item drop chance")]
    [SerializeField] private float dropChance;

    public Item Item => item;
    public float DropChance => dropChance;
}
