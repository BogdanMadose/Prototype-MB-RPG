using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [Tooltip("Slot prefab")]
    [SerializeField] private GameObject slotPrefab;
    private CanvasGroup _canvasGroup;
    private List<SlotScript> _slots = new List<SlotScript>();

    public bool IsOpen => _canvasGroup.alpha > 0;
    public int EmptyBagSlotCount
    {
        get
        {
            int count = 0;
            foreach (SlotScript slot in Slots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }
            return count;
        }
    }
    public List<SlotScript> Slots => _slots;
    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    /// <summary>
    /// Handles adding slots to bag
    /// </summary>
    /// <param name="slotCount">Number of slots</param>
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.BagInSlot = this;
            Slots.Add(slot);
        }
    }

    /// <summary>
    /// Handles adding items to bag
    /// </summary>
    /// <param name="item">Desired item(s)</param>
    /// <returns>
    /// <para>TRUE - Item can be added / Slot empty</para>
    /// <para>FALSE - Item cannot be added / Slot filled</para>
    /// </returns>
    public bool AddItemToBag(Item item)
    {
        foreach (SlotScript slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItemToSlot(item);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Handles displaying of specific bag
    /// </summary>
    public void OpenClose()
    {
        _canvasGroup.alpha = _canvasGroup.alpha > 0 ? 0 : 1;
        _canvasGroup.blocksRaycasts = _canvasGroup.blocksRaycasts != true;
    }

    /// <summary>
    /// Handles filling empty slots of original bag if bag destroyed
    /// </summary>
    /// <returns>Transfer items</returns>
    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach (SlotScript slot in _slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.Items)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }

    public void ClearBag()
    {
        foreach (SlotScript slot in _slots)
        {
            slot.TrashItems();
        }
    }
}
