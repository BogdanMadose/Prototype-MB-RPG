using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
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

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.BagInSlot = this;
            Slots.Add(slot);
        }
    }

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

    public void OpenClose()
    {
        _canvasGroup.alpha = _canvasGroup.alpha > 0 ? 0 : 1;
        _canvasGroup.blocksRaycasts = _canvasGroup.blocksRaycasts != true;
    }

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
}
