using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [Tooltip("Looted items")]
    [SerializeField] private Loot[] loot;
    private List<Item> _droppedItems = new List<Item>();
    private bool _rolled = false;

    /// <summary>
    /// Display loot items
    /// </summary>
    public void ShowLoot()
    {
        if (!_rolled)
        {
            RollLoot();
        }
        LootWindow.Instance.CreatePages(_droppedItems);
    }

    /// <summary>
    /// Set items to drop
    /// </summary>
    private void RollLoot()
    {
        foreach (Loot lootedItem in loot)
        {
            int roll = Random.Range(0, 100);
            if (roll <= lootedItem.DropChance)
            {
                _droppedItems.Add(lootedItem.Item);
            }
        }
        _rolled = true;
    }
}
